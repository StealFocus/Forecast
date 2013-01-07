namespace StealFocus.Forecast.Configuration.WindowsAzure.HostedService
{
    using System.Collections.ObjectModel;

    public class ScheduledHorizontalScaleConfiguration
    {
        public ScheduledHorizontalScaleConfiguration()
        {
            this.Schedules = new Collection<ScheduleDefinitionConfiguration>();
        }

        public string ServiceName { get; set; }

        public string SubscriptionConfigurationId { get; set; }

        public int PollingIntervalInMinutes { get; set; }

        public string DeploymentSlot { get; set; }
        
        public string RoleName { get; set; }
        
        public int InstanceCount { get; set; }
        
        public bool TreatWarningsAsError { get; set; }

        public string Mode { get; set; }

        public Collection<ScheduleDefinitionConfiguration> Schedules { get; private set; }
    }
}
