namespace StealFocus.Forecast.Configuration
{
    using System;
    using System.Globalization;

    public partial class WindowsAzureSubscriptionConfigurationElement
    {
        public Guid GetWindowsAzureSubscriptionId()
        {
            Guid id;
            bool parsed = Guid.TryParse(this.SubscriptionId, out id);
            if (parsed)
            {
                return id;
            }

            string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The Windows Azure Subscription ID of '{0}' found in the configuration could not be parsed as a GUID.", this.SubscriptionId);
            throw new ForecastException(exceptionMessage);
        }
    }
}
