namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;

    public static class HostedService
    {
        public static void DoSomething()
        {
            // X.509 certificate variables.
            X509Store certStore = null;
            X509Certificate2Collection certCollection = null;
            X509Certificate2 certificate = null;

            // Request and response variables.
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            // Stream variables.
            Stream responseStream = null;
            StreamReader reader = null;

            // URI variable.
            Uri requestUri = null;

            // Specify operation to use for the service management call.
            // This sample will use the operation for listing the hosted services.
            string operation = "hostedservices";

            // The ID for the Windows Azure subscription.
            string subscriptionId = "???";

            // The thumbprint for the certificate. This certificate would have been
            // previously added as a management certificate within the Windows Azure management portal.
            string thumbPrint = "194E98FC3435855FF3CFA6B4D3EA8273CAFF3ABA";

            // Open the certificate store for the current user.
            certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);

            // Find the certificate with the specified thumbprint.
            certCollection = certStore.Certificates.Find(
                                 X509FindType.FindByThumbprint,
                                 thumbPrint,
                                 false);

            // Close the certificate store.
            certStore.Close();

            // Check to see if a matching certificate was found.
            if (0 == certCollection.Count)
            {
                throw new ForecastException("No certificate found containing thumbprint " + thumbPrint);
            }

            // A matching certificate was found.
            certificate = certCollection[0];
            Console.WriteLine("Using certificate with thumbprint: " + thumbPrint);

            // Create the request.
            requestUri = new Uri("https://management.core.windows.net/"
                                 + subscriptionId
                                 + "/services/"
                                 + operation);

            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUri);

            // Add the certificate to the request.
            httpWebRequest.ClientCertificates.Add(certificate);

            // Specify the version information in the header.
            httpWebRequest.Headers.Add("x-ms-version", "2011-10-01");

            // Make the call using the web request.
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            // Display the web response status code.
            Console.WriteLine("Response status code: " + httpWebResponse.StatusCode);

            // Display the request ID returned by Windows Azure.
            if (null != httpWebResponse.Headers)
            {
                Console.WriteLine("x-ms-request-id: "
                + httpWebResponse.Headers["x-ms-request-id"]);
            }

            // Parse the web response.
            responseStream = httpWebResponse.GetResponseStream();
            reader = new StreamReader(responseStream);

            // Display the raw response.
            Console.WriteLine("Response output:");
            Console.WriteLine(reader.ReadToEnd());

            // Close the resources no longer needed.
            httpWebResponse.Close();
            reader.Close();
        }
    }
}
