namespace StealFocus.Forecast.Configuration.WindowsAzure.HostedService
{
    using System.Collections.ObjectModel;

    public class WhiteListService
    {
        public WhiteListService()
        {
            this.Roles = new Collection<WhiteListRole>();
        }

        public string Name { get; set; }

        public Collection<WhiteListRole> Roles { get; private set; }
    }
}
