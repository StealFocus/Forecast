namespace StealFocus.Forecast.Configuration.WindowsAzure.HostedService
{
    using StealFocus.Forecast.WindowsAzure.HostedService;

    public class WhiteListRole
    {
        public string Name { get; set; }

        public InstanceSize? MaxInstanceSize { get; set; }

        public int? MaxInstanceCount { get; set; }
    }
}
