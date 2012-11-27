namespace StealFocus.Forecast.Configuration
{
    using System;
    using System.Globalization;

    public partial class DayConfigurationElement
    {
        public DayOfWeek GetDayOfWeek()
        {
            DayOfWeek dayOfWeek;
            bool parsed = Enum.TryParse(this.Name, out dayOfWeek);
            if (parsed)
            {
                return dayOfWeek;
            }

            string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The configured 'name' value of '{0}' found in the configuration could not be parsed as a 'DayOfWeek'.", this.Name);
            throw new ForecastException(exceptionMessage);
        }
    }
}
