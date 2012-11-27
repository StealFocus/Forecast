namespace StealFocus.Forecast.Configuration
{
    using System;

    public class DayConfiguration
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
