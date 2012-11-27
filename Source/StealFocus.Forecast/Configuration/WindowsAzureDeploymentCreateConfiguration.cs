namespace StealFocus.Forecast.Configuration
{
    using System.Collections.ObjectModel;

    public class WindowsAzureDeploymentCreateConfiguration
    {
        public WindowsAzureDeploymentCreateConfiguration()
        {
            this.Schedules = new Collection<ScheduleConfiguration>();
        }

        public string ServiceName { get; set; }

        public string Id { get; set; }

        public string SubscriptionConfigurationId { get; set; }

        public int PollingIntervalInMinutes { get; set; }

        public string DeploymentSlot { get; set; }

        public string WindowsAzurePackageId { get; set; }

        public string DeploymentName { get; set; }

        public string DeploymentLabel { get; set; }

        public string PackageConfigurationFilePath { get; set; }

        public bool StartDeployment { get; set; }

        public bool TreatWarningsAsError { get; set; }

        public Collection<ScheduleConfiguration> Schedules { get; private set; }
    }
}
