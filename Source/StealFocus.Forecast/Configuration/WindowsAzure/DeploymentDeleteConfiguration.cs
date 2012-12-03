namespace StealFocus.Forecast.Configuration.WindowsAzure
{
    using System.Collections.ObjectModel;

    public class DeploymentDeleteConfiguration
    {
        public DeploymentDeleteConfiguration()
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
