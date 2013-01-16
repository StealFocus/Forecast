namespace StealFocus.Forecast.Configuration.WindowsAzure.HostedService
{
    using System.Collections.ObjectModel;

    public class WhiteListConfiguration
    {
        public WhiteListConfiguration()
        {
            this.ServiceNames = new Collection<string>();
        }

        public int PollingIntervalInMinutes { get; set; }

        public bool IncludeDeploymentDeleteServices { get; set; }

        public bool IncludeDeploymentCreateServices { get; set; }

        public bool IncludeHorizontalScaleServices { get; set; }

        public Collection<string> ServiceNames { get; private set; }
    }
}
