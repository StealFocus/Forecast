namespace StealFocus.Forecast.Configuration.WindowsAzure.StorageService
{
    using System.Collections.ObjectModel;

    public class TableDeleteConfiguration
    {
        public TableDeleteConfiguration()
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
