namespace StealFocus.Forecast.WindowsAzure
{
    using System;
    using System.Globalization;

    public static class GuidExtensions
    {
        public static string AzureRestFormat(this Guid subscriptionId)
        {
            return subscriptionId.ToString("D", CultureInfo.CurrentCulture);
        }
    }
}
