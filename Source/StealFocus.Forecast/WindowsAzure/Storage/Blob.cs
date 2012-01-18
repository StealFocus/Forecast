namespace StealFocus.Forecast.WindowsAzure.Storage
{
    using System;
    using System.Globalization;

    public static class Blob
    {
        public static Uri GetUrl(string storageAccountName, string containerName, string blobName)
        {
            // http://<storageAccountName>.blob.core.windows.net/<containerName>/<blobName>
            string blobUrl = string.Format(CultureInfo.CurrentCulture, "http://{0}.blob.core.windows.net/{1}/{2}", storageAccountName, containerName, blobName);
            return new Uri(blobUrl);
        }
    }
}
