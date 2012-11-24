namespace StealFocus.Forecast.Configuration
{
    using System.Collections;

    using StealFocus.AzureExtensions.HostedService;

    using WindowsAzure;

    public partial class StealFocusForecastConfiguration
    {
        internal DeploymentDeleteForecastWorker[] GetDeploymentDeleteForecastWorkers()
        {
            ArrayList list = new ArrayList(this.WindowsAzureDeploymentDeletes.Count);
            foreach (WindowsAzureDeploymentDeleteConfigurationElement windowsAzureDeploymentDelete in this.WindowsAzureDeploymentDeletes)
            {
                WindowsAzureSubscriptionConfigurationElement windowsAzureSubscription = this.WindowsAzureSubscriptions[windowsAzureDeploymentDelete.SubscriptionConfigurationId];
                
                // Fix
                DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                    new Deployment(), 
                    new Operation(), 
                    windowsAzureSubscription.GetWindowsAzureSubscriptionId(), 
                    windowsAzureSubscription.CertificateThumbprint, 
                    windowsAzureDeploymentDelete.ServiceName, 
                    windowsAzureDeploymentDelete.DeploymentSlot,
                    windowsAzureDeploymentDelete.DailyStartTime,
                    windowsAzureDeploymentDelete.DailyEndTime,
                    windowsAzureDeploymentDelete.PollingIntervalInMinutes);
                list.Add(deploymentDeleteForecastWorker);
            }

            return (DeploymentDeleteForecastWorker[])list.ToArray(typeof(DeploymentDeleteForecastWorker));
        }
    }
}
