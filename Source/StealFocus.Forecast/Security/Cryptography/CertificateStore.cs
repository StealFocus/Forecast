namespace StealFocus.Forecast.Security.Cryptography
{
    using System.Globalization;
    using System.Security.Cryptography.X509Certificates;

    public static class CertificateStore
    {
        public static X509Certificate2 GetCertificateFromCurrentUserStore(string certificateThumbprint)
        {
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificateCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false);
            certStore.Close();
            if (certificateCollection.Count == 0)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "No X509 certificate was found in the user's certificate store matching thumbprint '{0}'.", certificateThumbprint);
                throw new ForecastException(exceptionMessage);
            }

            return certificateCollection[0];
        }
    }
}
