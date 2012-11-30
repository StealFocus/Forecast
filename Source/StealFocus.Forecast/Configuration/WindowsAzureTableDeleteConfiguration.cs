namespace StealFocus.Forecast.Configuration
{
    using System.Collections.ObjectModel;

    public class WindowsAzureTableDeleteConfiguration
    {
        public WindowsAzureTableDeleteConfiguration()
        {
            this.TableNames = new Collection<string>();
            this.Schedules = new Collection<ScheduleDefinitionConfiguration>();
        }

        public string StorageAccountName { get; set; }

        public string StorageAccountKey { get; set; }

        public int PollingIntervalInMinutes { get; set; }

        public Collection<string> TableNames { get; private set;  }

        public Collection<ScheduleDefinitionConfiguration> Schedules { get; private set; }
    }
}
