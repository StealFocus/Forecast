namespace StealFocus.Forecast.Configuration
{
    using System;
    using System.Collections;

    using StealFocus.AzureExtensions.HostedService;
    using StealFocus.AzureExtensions.StorageService;

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
                    foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureDeploymentDeleteConfigurationElement.Schedules)
                    {
                        DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                            windowsAzureDeploymentDeleteConfigurationElement.Id,
                            new Deployment(),
                            new Operation(),
                            windowsAzureSubscriptionConfigurationElement.GetWindowsAzureSubscriptionId(),
                            windowsAzureSubscriptionConfigurationElement.CertificateThumbprint,
                            windowsAzureDeploymentDeleteConfigurationElement.ServiceName,
                            deploymentSlotConfigurationElement.Name,
                            scheduleConfigurationElement.DailyStartTime,
                            scheduleConfigurationElement.DailyEndTime,
                            windowsAzureDeploymentDeleteConfigurationElement.PollingIntervalInMinutes);
                        list.Add(deploymentDeleteForecastWorker);
                    }
                }
            }

            return (DeploymentDeleteForecastWorker[])list.ToArray(typeof(DeploymentDeleteForecastWorker));
        }

        internal DeploymentCreateForecastWorker[] GetDeploymentCreateForecastWorkers()
        {
            ArrayList list = new ArrayList();
            foreach (WindowsAzureDeploymentCreateConfigurationElement windowsAzureDeploymentCreateConfigurationElement in this.WindowsAzureDeploymentCreates)
            {
                WindowsAzureSubscriptionConfigurationElement windowsAzureSubscriptionConfigurationElement = this.WindowsAzureSubscriptions[windowsAzureDeploymentCreateConfigurationElement.SubscriptionConfigurationId];
                WindowsAzurePackageConfigurationElement windowsAzurePackageConfigurationElement = this.WindowsAzurePackages[windowsAzureDeploymentCreateConfigurationElement.WindowsAzurePackageId];
                foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureDeploymentCreateConfigurationElement.Schedules)
                {
                    Uri packageUrl = Blob.GetUrl(windowsAzurePackageConfigurationElement.StorageAccountName, windowsAzurePackageConfigurationElement.ContainerName, windowsAzurePackageConfigurationElement.BlobName);
                    DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                        windowsAzureDeploymentCreateConfigurationElement.Id,
                        new Deployment(),
                        new Operation(),
                        windowsAzureSubscriptionConfigurationElement.GetWindowsAzureSubscriptionId(),
                        windowsAzureSubscriptionConfigurationElement.CertificateThumbprint,
                        windowsAzureDeploymentCreateConfigurationElement.ServiceName,
                        windowsAzureDeploymentCreateConfigurationElement.DeploymentSlot,
                        scheduleConfigurationElement.DailyStartTime,
                        scheduleConfigurationElement.DailyEndTime,
                        windowsAzureDeploymentCreateConfigurationElement.DeploymentName,
                        packageUrl,
                        windowsAzureDeploymentCreateConfigurationElement.DeploymentLabel,
                        windowsAzureDeploymentCreateConfigurationElement.PackageConfigurationFilePath,
                        windowsAzureDeploymentCreateConfigurationElement.StartDeployment,
                        windowsAzureDeploymentCreateConfigurationElement.TreatWarningsAsError,
                        windowsAzureDeploymentCreateConfigurationElement.PollingIntervalInMinutes);
                    list.Add(deploymentCreateForecastWorker);
                }
            }

            return (DeploymentCreateForecastWorker[])list.ToArray(typeof(DeploymentCreateForecastWorker));
        }
    }
}
