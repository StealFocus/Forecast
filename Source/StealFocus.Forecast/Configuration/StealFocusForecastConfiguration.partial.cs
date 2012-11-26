namespace StealFocus.Forecast.Configuration
{
    using System;
    using System.Collections;
    using System.Globalization;

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
                        ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfigurationElement(scheduleConfigurationElement);
                        DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                            windowsAzureDeploymentDeleteConfigurationElement.Id,
                            new Deployment(),
                            new Operation(),
                            windowsAzureSubscriptionConfigurationElement.GetWindowsAzureSubscriptionId(),
                            windowsAzureSubscriptionConfigurationElement.CertificateThumbprint,
                            windowsAzureDeploymentDeleteConfigurationElement.ServiceName,
                            deploymentSlotConfigurationElement.Name,
                            scheduleDays,
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
                    ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfigurationElement(scheduleConfigurationElement);
                    Uri packageUrl = Blob.GetUrl(windowsAzurePackageConfigurationElement.StorageAccountName, windowsAzurePackageConfigurationElement.ContainerName, windowsAzurePackageConfigurationElement.BlobName);
                    DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                        windowsAzureDeploymentCreateConfigurationElement.Id,
                        new Deployment(),
                        new Operation(),
                        windowsAzureSubscriptionConfigurationElement.GetWindowsAzureSubscriptionId(),
                        windowsAzureSubscriptionConfigurationElement.CertificateThumbprint,
                        windowsAzureDeploymentCreateConfigurationElement.ServiceName,
                        windowsAzureDeploymentCreateConfigurationElement.DeploymentSlot,
                        scheduleDays,
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

        private static ScheduleDay[] GetScheduleDaysFromScheduleConfigurationElement(ScheduleConfigurationElement scheduleConfigurationElement)
        {
            ScheduleDay[] scheduleDays = new ScheduleDay[scheduleConfigurationElement.Days.Count];
            for (int i = 0; i < scheduleConfigurationElement.Days.Count; i++)
            {
                DayOfWeek dayOfWeek;
                bool tryParseSuccess = Enum.TryParse(scheduleConfigurationElement.Days[i].Name, true, out dayOfWeek);
                if (!tryParseSuccess)
                {
                    string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The configured schedule day name of '{0}' could not be parsed as a valid day of the week.", scheduleConfigurationElement.Days[i].Name);
                    throw new ForecastException(exceptionMessage);
                }

                scheduleDays[i] = new ScheduleDay
                    {
                        DayOfWeek = dayOfWeek,
                        EndTime = scheduleConfigurationElement.Days[i].EndTime,
                        StartTime = scheduleConfigurationElement.Days[i].StartTime
                    };
            }

            return scheduleDays;
        }
    }
}
