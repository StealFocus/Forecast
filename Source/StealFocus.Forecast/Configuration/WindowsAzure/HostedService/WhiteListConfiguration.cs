namespace StealFocus.Forecast.Configuration.WindowsAzure.HostedService
{
    using System.Collections.ObjectModel;

    public class WhiteListConfiguration
    {
        public WhiteListConfiguration()
        {
            this.Services = new Collection<WhiteListService>();
        }

        public int PollingIntervalInMinutes { get; set; }

        public bool IncludeDeploymentDeleteServices { get; set; }

        public bool IncludeDeploymentCreateServices { get; set; }

        public bool IncludeHorizontalScaleServices { get; set; }

        public Collection<WhiteListService> Services { get; private set; }
    }
}
