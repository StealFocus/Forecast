namespace StealFocus.Forecast.Configuration.WindowsAzure
{
    using System.Collections.ObjectModel;

    public class DeploymentCreateConfiguration
    {
        public DeploymentCreateConfiguration()
        {
            this.Schedules = new Collection<ScheduleDefinitionConfiguration>();
        }

        public string ServiceName { get; set; }

        public string SubscriptionConfigurationId { get; set; }

        public int PollingIntervalInMinutes { get; set; }

        public string DeploymentSlot { get; set; }

        public string WindowsAzurePackageId { get; set; }

        public string DeploymentName { get; set; }

        public string DeploymentLabel { get; set; }

        public string PackageConfigurationFilePath { get; set; }

        public bool StartDeployment { get; set; }

        public bool TreatWarningsAsError { get; set; }

        public Collection<ScheduleDefinitionConfiguration> Schedules { get; private set; }
    }
}
