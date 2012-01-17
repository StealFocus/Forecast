namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;

    public static class Deployment
    {
        public static void Delete()
        {
            // https://management.core.windows.net/<subscription-id>/services/hostedservices/<service-name>/deploymentslots/<deployment-slot>
            // https://management.core.windows.net/<subscription-id>/services/hostedservices/<service-name>/deployments/<deployment-name>
            // Deployment Slot = "Production" or "Staging"
            // Deployment Name = "Beazley.Authority v1.0.20117.0 (Debug)"
            // "x-ms-version" = "2011-12-01"
            const string SubscriptionId = "???";
            const string ServiceName = "BeazleyTasks-WEuro-Sys";
            const string DeploymentSlot = "Production";

            // Build a URI
            Uri requestUri = new Uri("https://management.core.windows.net/" + SubscriptionId + "/services/hostedservices/" + ServiceName + "/deploymentslots/" + DeploymentSlot);

            // Create the request and specify attributes of the request.
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);

            // Define the requred headers to specify the API version and operation type.
            request.Headers.Add("x-ms-version", "2011-12-01");
            request.Method = "DELETE";
            request.ContentType = "application/xml";

            // The thumbprint value of the management certificate.
            // You must replace the string with the thumbprint of a 
            // management certificate associated with your subscription.
            const string CertThumbprint = "194E98FC3435855FF3CFA6B4D3EA8273CAFF3ABA";

            // Create a reference to the My certificate store.
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            // Try to open the store.
            certStore.Open(OpenFlags.ReadOnly);
            
            // Find the certificate that matches the thumbprint.
            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, CertThumbprint, false);
            certStore.Close();

            // Check to see if our certificate was added to the collection. If no, throw an error, if yes, create a certificate using it.
            if (0 == certCollection.Count)
            {
                throw new ForecastException("Error: No certificate found containing thumbprint " + CertThumbprint);
            }

            // Create an X509Certificate2 object using our matching certificate.
            X509Certificate2 certificate = certCollection[0];

            // Attach the certificate to the request.
            request.ClientCertificates.Add(certificate);

            // Make the call using the web request.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Display the web response status code.
            Console.WriteLine("Response status code: " + response.StatusCode);

            // Display the request ID returned by Windows Azure.
            if (null != response.Headers)
            {
                Console.WriteLine("x-ms-request-id: " + response.Headers["x-ms-request-id"]);
            }

            // Parse the web response.
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                throw new ForecastException();
            }

            StreamReader reader = new StreamReader(responseStream);

            // Display the raw response.
            Console.WriteLine("Response output:");
            Console.WriteLine(reader.ReadToEnd());

            // Close the resources no longer needed.
            response.Close();
            reader.Close();
        }
    }
}
