// -----------------------------------------------------------------------
// <copyright file="WindowsAzureBlobContainerDeleteConfiguration.cs" company="Beazley">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace StealFocus.Forecast.Configuration
{
    using System.Collections.ObjectModel;

    public class WindowsAzureBlobContainerDeleteConfiguration
    {
        public WindowsAzureBlobContainerDeleteConfiguration()
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
