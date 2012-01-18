namespace StealFocus.Forecast.WindowsAzure
{
    using System.Globalization;

    public static class BooleanExtensions
    {
        public static string AzureRestFormat(this bool value)
        {
            return value.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture);
        }
    }
}
