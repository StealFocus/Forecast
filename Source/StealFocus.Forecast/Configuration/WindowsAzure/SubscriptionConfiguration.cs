namespace StealFocus.Forecast.Configuration.WindowsAzure
{
    using System;

    public class SubscriptionConfiguration
    {
        public Guid SubscriptionId { get; set; }

        public string CertificateThumbprint { get; set; }

        public string Id { get; set; }
    }
}
