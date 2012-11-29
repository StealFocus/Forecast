namespace StealFocus.Forecast.Configuration
{
    using System.Collections.ObjectModel;

    public class WindowsAzureDeploymentDeleteConfiguration
    {
        public WindowsAzureDeploymentDeleteConfiguration()
        {
            this.DeploymentSlots = new Collection<string>();
            this.Schedules = new Collection<ScheduleDefinitionConfiguration>();
        }

        public string ServiceName { get; set; }

        public string SubscriptionConfigurationId { get; set; }

        public int PollingIntervalInMinutes { get; set; }

        public Collection<string> DeploymentSlots { get; private set; }

        public Collection<ScheduleDefinitionConfiguration> Schedules { get; private set; }
    }
}
