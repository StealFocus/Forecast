namespace StealFocus.Forecast.Configuration.WindowsAzure
{
    using System.Collections.ObjectModel;

    public class BlobContainerDeleteConfiguration
    {
        public BlobContainerDeleteConfiguration()
        {
            this.BlobContainerNames = new Collection<string>();
            this.Schedules = new Collection<ScheduleDefinitionConfiguration>();
        }

        public string StorageAccountName { get; set; }

        public string StorageAccountKey { get; set; }

        public int PollingIntervalInMinutes { get; set; }

        public Collection<string> BlobContainerNames { get; private set;  }

        public Collection<ScheduleDefinitionConfiguration> Schedules { get; private set; }
    }
}
