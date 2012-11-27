namespace StealFocus.Forecast.Configuration
{
    using System;

    public class WindowsAzureSubscriptionConfiguration
    {
        public Guid SubscriptionId { get; set; }

        public string CertificateThumbprint { get; set; }

        public string Id { get; set; }
    }
}
