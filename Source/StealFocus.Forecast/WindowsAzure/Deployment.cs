namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Xml.Linq;
    using Net;
    using Security.Cryptography;

    public static class Deployment
    {
        /// <param name="subscriptionId">The SubScription ID.</param>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <param name="serviceName">The service name.</param>
        /// <param name="deploymentSlot">Either "Production" or "Staging".</param>
        public static string DeleteRequest(Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot)
        {
            HttpWebRequest httpWebRequest = GetRequestForDelete(subscriptionId, certificateThumbprint, serviceName, deploymentSlot);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
                string exceptionMessage;
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    exceptionMessage = string.Format(CultureInfo.CurrentCulture, "There was en error deleting deployment for service '{0}' in deployment slot '{1}', the service and deployment slot combination was not found.", serviceName, deploymentSlot);
                }
                else
                {
                    exceptionMessage = string.Format(CultureInfo.CurrentCulture, "There was en error deleting deployment for service '{0}' in deployment slot '{1}'.", serviceName, deploymentSlot);                    
                }

                throw new ForecastException(exceptionMessage, e);
            }

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The HTTP Status Code returned in the response was '{0}', expected was '{1}'.", response.StatusCode, HttpStatusCode.Accepted);
                throw new ForecastException(exceptionMessage);
            }

            return response.Headers[ResponseHeaderName.MSRequestId];
        }

        /// <param name="subscriptionId">The SubScription ID.</param>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <param name="serviceName">The service name.</param>
        /// <param name="deploymentSlot">Either "Production" or "Staging".</param>
        /// <param name="deploymentName">Shoud not contain spaces.</param>
        /// <param name="packageUrl">The URL to the .cspkg in blob storage.</param>
        /// <param name="label">Limited to 100 characters.</param>
        /// <param name="configurationFilePath">The path to the .cscfg file.</param>
        /// <param name="startDeployment">Whether to start after deployment.</param>
        /// <param name="treatWarningsAsError">Whether to treat warnings as errors.</param>
        public static string CreateRequest(Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot, string deploymentName, Uri packageUrl, string label, string configurationFilePath, bool startDeployment, bool treatWarningsAsError)
        {
            HttpWebRequest httpWebRequest = GetRequestForCreate(subscriptionId, certificateThumbprint, serviceName, deploymentSlot, deploymentName, packageUrl, label, configurationFilePath, startDeployment, treatWarningsAsError);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
                ForecastAzureOperationException forecastAzureOperationException = new ForecastAzureOperationException();
                forecastAzureOperationException.ResponseBody = response.GetResponseBody();
                throw forecastAzureOperationException;
            }

            return response.Headers[ResponseHeaderName.MSRequestId];
        }

        private static HttpWebRequest GetRequestForDelete(Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot)
        {
            string deleteDeploymentUrl = GetDeleteDeploymentUrl(subscriptionId.AzureRestFormat(), serviceName, deploymentSlot);
            Uri requestUri = new Uri(deleteDeploymentUrl);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            httpWebRequest.Headers.Add(RequestHeaderName.MSVersion, RequestMSVersion.December2011);
            httpWebRequest.Method = RequestMethod.Delete;
            httpWebRequest.ContentType = RequestContentType.ApplicationXml;
            X509Certificate2 certificate = CertificateStore.GetCertificateFromCurrentUserStore(certificateThumbprint);
            httpWebRequest.ClientCertificates.Add(certificate);
            return httpWebRequest;
        }

        private static HttpWebRequest GetRequestForCreate(Guid subscriptionId, string certificateThumbprint, string serviceName, string deploymentSlot, string deploymentName, Uri packageUrl, string label, string configurationFilePath, bool startDeployment, bool treatWarningsAsError)
        {
            string deleteDeploymentUrl = GetCreateDeploymentUrl(subscriptionId.AzureRestFormat(), serviceName, deploymentSlot);
            Uri requestUri = new Uri(deleteDeploymentUrl);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            httpWebRequest.Headers.Add(RequestHeaderName.MSVersion, RequestMSVersion.December2011);
            httpWebRequest.Method = RequestMethod.Post;
            httpWebRequest.ContentType = RequestContentType.ApplicationXml;
            X509Certificate2 certificate = CertificateStore.GetCertificateFromCurrentUserStore(certificateThumbprint);
            httpWebRequest.ClientCertificates.Add(certificate);
            XDocument requestBody = GetCreateDeploymentRequestBody(deploymentName, packageUrl, label, configurationFilePath, startDeployment, treatWarningsAsError);
            Stream requestStream = null;
            try
            {
                requestStream = httpWebRequest.GetRequestStream();
                using (StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.UTF8))
                {
                    requestStream = null;
                    requestBody.Save(streamWriter, SaveOptions.DisableFormatting);
                }
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Dispose();
                }
            }

            return httpWebRequest;
        }

        private static string GetDeleteDeploymentUrl(string subscriptionId, string serviceName, string deploymentSlot)
        {
            return GetDeploymentUrl(subscriptionId, serviceName, deploymentSlot);
        }

        private static string GetCreateDeploymentUrl(string subscriptionId, string serviceName, string deploymentSlot)
        {
            return GetDeploymentUrl(subscriptionId, serviceName, deploymentSlot);
        }

        private static string GetDeploymentUrl(string subscriptionId, string serviceName, string deploymentSlot)
        {
            // https://management.core.windows.net/<subscriptionId>/services/hostedservices/<serviceName>/deploymentslots/<deploymentSlot>
            return string.Format(CultureInfo.CurrentCulture, "https://management.core.windows.net/{0}/services/hostedservices/{1}/deploymentslots/{2}", subscriptionId, serviceName, deploymentSlot);
        }

        private static XDocument GetCreateDeploymentRequestBody(string name, Uri packageUrl, string label, string configurationFilePath, bool startDeployment, bool treatWarningsAsError)
        {
            /*
             *  <?xml version="1.0" encoding="utf-8"?>
             *  <CreateDeployment xmlns="http://schemas.microsoft.com/windowsazure">
             *      <Name>deployment-name</Name>
             *      <PackageUrl>package-url-in-blob-storage</PackageUrl>
             *      <Label>base64-encoded-deployment-label</Label>
             *      <Configuration>base64-encoded-configuration-file</Configuration>
             *      <StartDeployment>true|false</StartDeployment>
             *      <TreatWarningsAsError>true|false</TreatWarningsAsError>
             *  </CreateDeployment>
            */
            XNamespace windowsAzureNamespace = XmlNamespace.MicrosoftWindowsAzure;
            XDocument requestBody = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(
                    windowsAzureNamespace + "CreateDeployment",
                    new XElement(windowsAzureNamespace + "Name", name),
                    new XElement(windowsAzureNamespace + "PackageUrl", packageUrl.AbsoluteUri),
                    new XElement(windowsAzureNamespace + "Label", label.ToBase64()),
                    new XElement(windowsAzureNamespace + "Configuration", GetConfigurationFileAsSingleString(configurationFilePath).ToBase64()),
                    new XElement(windowsAzureNamespace + "StartDeployment", startDeployment.AzureRestFormat()),
                    new XElement(windowsAzureNamespace + "TreatWarningsAsError", treatWarningsAsError.AzureRestFormat())));
            return requestBody;
        }

        private static string GetConfigurationFileAsSingleString(string configurationFilePath)
        {
            return string.Join(string.Empty, File.ReadAllLines(configurationFilePath));
        }
    }
}
