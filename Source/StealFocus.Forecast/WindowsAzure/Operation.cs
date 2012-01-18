namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Xml.Linq;
    using Net;
    using Security.Cryptography;

    public static class Operation
    {
        public static OperationResult StatusCheck(Guid subscriptionId, string certificateThumbprint, string requestId)
        {
            HttpWebRequest httpWebRequest = GetRequestForStatusCheck(subscriptionId, certificateThumbprint, requestId);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException e)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "There was an error querying the operation status of Request ID '{0}'.", requestId);
                throw new ForecastException(exceptionMessage, e);
            }

            OperationResult operationResult = ExtractOperationResultFromResponse(response);
            return operationResult;
        }

        private static HttpWebRequest GetRequestForStatusCheck(Guid subscriptionId, string certificateThumbprint, string requestId)
        {
            string statusCheckUrl = GetStatusCheckUrl(subscriptionId.AzureRestFormat(), requestId);
            Uri requestUri = new Uri(statusCheckUrl);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            httpWebRequest.Headers.Add(RequestHeaderName.MSVersion, RequestMSVersion.December2011);
            httpWebRequest.Method = RequestMethod.Get;
            httpWebRequest.ContentType = RequestContentType.ApplicationXml;
            X509Certificate2 certificate = CertificateStore.GetCertificateFromCurrentUserStore(certificateThumbprint);
            httpWebRequest.ClientCertificates.Add(certificate);
            return httpWebRequest;
        }

        private static string GetStatusCheckUrl(string subscriptionId, string requestId)
        {
            // https://management.core.windows.net/<subscriptionId>/operations/<requestId>
            return string.Format(CultureInfo.CurrentCulture, "https://management.core.windows.net/{0}/operations/{1}", subscriptionId, requestId);
        }

        private static OperationResult ExtractOperationResultFromResponse(HttpWebResponse httpWebResponse)
        {
            XDocument responseBody = httpWebResponse.GetResponseBody();
            /*
             *  <Operation xmlns="http://schemas.microsoft.com/windowsazure" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
             *      <ID>[GUID]</ID>
             *      <Status>InProgress|Succeeded|Failed</Status>
             *      <HttpStatusCode>200|etc</HttpStatusCode>
             *      <Code>code</Code>
             *      <Message>text</Message>
             *  </Operation>
             */
            OperationResult operationResult = new OperationResult();
            if (responseBody.Root == null)
            {
                throw new ForecastException("The response did not contain a root element.");
            }

            operationResult.Id = Guid.Parse(GetChild(responseBody.Root, "ID").Value);
            operationResult.Status = (OperationStatus)Enum.Parse(typeof(OperationStatus), GetChild(responseBody.Root, "Status").Value);
            operationResult.HttpStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), GetChild(responseBody.Root, "HttpStatusCode").Value);
            if (operationResult.Status == OperationStatus.Failed)
            {
                operationResult.Code = GetChild(responseBody.Root, "Code").Value;
                operationResult.Message = GetChild(responseBody.Root, "Message").Value;
            }
            
            return operationResult;
        }

        private static XElement GetChild(XElement parentElement, string childElementName)
        {
            XElement element = parentElement.Element(childElementName);
            if (element == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The response did not contain a '{0}' element under the root as expected.", childElementName);
                throw new ForecastException(exceptionMessage);
            }

            return element;
        }
    }
}
