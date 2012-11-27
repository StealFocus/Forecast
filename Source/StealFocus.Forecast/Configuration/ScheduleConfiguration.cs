namespace StealFocus.Forecast.Configuration
{
    using System.Collections.ObjectModel;

    public class ScheduleConfiguration
    {
        public ScheduleConfiguration()
        {
            this.Days = new Collection<DayConfiguration>();
        }

        public string Name { get; set; }

        public Collection<DayConfiguration> Days { get; private set; }
    }
}
