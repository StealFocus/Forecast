namespace StealFocus.Forecast.Configuration
{
    using System.Collections.ObjectModel;

    public class WindowsAzureDeploymentDeleteConfiguration
    {
        public WindowsAzureDeploymentDeleteConfiguration()
        {
            this.DeploymentSlots = new Collection<string>();
            this.Schedules = new Collection<ScheduleConfiguration>();
        }

        public string ServiceName { get; set; }

        public string SubscriptionConfigurationId { get; set; }

        public int PollingIntervalInMinutes { get; set; }

        public Collection<string> DeploymentSlots { get; private set; }

        public Collection<ScheduleConfiguration> Schedules { get; private set; }
    }
}
