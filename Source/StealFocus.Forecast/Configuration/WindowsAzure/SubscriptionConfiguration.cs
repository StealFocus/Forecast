namespace StealFocus.Forecast.Configuration.WindowsAzure
{
    using System;

    using StealFocus.AzureExtensions;

    public class SubscriptionConfiguration
    {
        public Guid SubscriptionId { get; set; }

        public string CertificateThumbprint { get; set; }

        public string Id { get; set; }

        internal ISubscription Convert()
        {
            return new Subscription(this.SubscriptionId, this.CertificateThumbprint);
        }
    }
}
