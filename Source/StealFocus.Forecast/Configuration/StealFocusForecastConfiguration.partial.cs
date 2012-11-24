namespace StealFocus.Forecast.Configuration
{
    using System.Collections;

    using StealFocus.AzureExtensions.HostedService;

    using WindowsAzure;

    public partial class StealFocusForecastConfiguration
    {
        internal DeploymentDeleteForecastWorker[] GetDeploymentDeleteForecastWorkers()
        {
            ArrayList list = new ArrayList();
            foreach (WindowsAzureDeploymentDeleteConfigurationElement windowsAzureDeploymentDeleteConfigurationElement in this.WindowsAzureDeploymentDeletes)
            {
                WindowsAzureSubscriptionConfigurationElement windowsAzureSubscriptionConfigurationElement = this.WindowsAzureSubscriptions[windowsAzureDeploymentDeleteConfigurationElement.SubscriptionConfigurationId];
                foreach (DeploymentSlotConfigurationElement deploymentSlotConfigurationElement in windowsAzureDeploymentDeleteConfigurationElement.DeploymentSlots)
                {
                    DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                        new Deployment(),
                        new Operation(),
                        windowsAzureSubscriptionConfigurationElement.GetWindowsAzureSubscriptionId(),
                        windowsAzureSubscriptionConfigurationElement.CertificateThumbprint,
                        windowsAzureDeploymentDeleteConfigurationElement.ServiceName,
                        deploymentSlotConfigurationElement.Name,
                        windowsAzureDeploymentDeleteConfigurationElement.DailyStartTime,
                        windowsAzureDeploymentDeleteConfigurationElement.DailyEndTime,
                        windowsAzureDeploymentDeleteConfigurationElement.PollingIntervalInMinutes);
                    list.Add(deploymentDeleteForecastWorker);
                }
            }

            return (DeploymentDeleteForecastWorker[])list.ToArray(typeof(DeploymentDeleteForecastWorker));
        }
    }
}
