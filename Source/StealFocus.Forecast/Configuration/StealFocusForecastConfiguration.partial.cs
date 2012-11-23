namespace StealFocus.Forecast.Configuration
{
    using System.Collections;

    using StealFocus.AzureExtensions.HostedService;

    using WindowsAzure;

    public partial class StealFocusForecastConfiguration
    {
        public DeploymentDeleteForecastWorker[] GetDeploymentDeleteForecastWorkers()
        {
            ArrayList list = new ArrayList(this.WindowsAzureDeploymentDeletes.Count);
            foreach (WindowsAzureDeploymentDeleteConfigurationElement windowsAzureDeploymentDelete in this.WindowsAzureDeploymentDeletes)
            {
                WindowsAzureSubscriptionConfigurationElement windowsAzureSubscription = this.WindowsAzureSubscriptions[windowsAzureDeploymentDelete.SubscriptionConfigurationId];
                DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                    new Deployment(), 
                    windowsAzureSubscription.GetWindowsAzureSubscriptionId(), 
                    windowsAzureSubscription.CertificateThumbprint, 
                    windowsAzureDeploymentDelete.ServiceName, 
                    windowsAzureDeploymentDelete.DeploymentSlot, 
                    windowsAzureDeploymentDelete.ScheduledTime);
                list.Add(deploymentDeleteForecastWorker);
            }

            return (DeploymentDeleteForecastWorker[])list.ToArray(typeof(DeploymentDeleteForecastWorker));
        }
    }
}
