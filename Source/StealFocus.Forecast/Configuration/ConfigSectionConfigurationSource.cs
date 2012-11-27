namespace StealFocus.Forecast.Configuration
{
    using System.Collections;
    using System.Collections.Generic;

    internal class ConfigSectionConfigurationSource : IConfigurationSource
    {
        public WindowsAzureSubscriptionConfiguration GetWindowsAzureSubscriptionConfiguration(string windowsAzureSubscriptionConfigurationId)
        {
            WindowsAzureSubscriptionConfigurationElement windowsAzureSubscriptionConfigurationElement = StealFocusForecastConfiguration.Instance.WindowsAzureSubscriptions[windowsAzureSubscriptionConfigurationId];
            return new WindowsAzureSubscriptionConfiguration
                {
                    CertificateThumbprint = windowsAzureSubscriptionConfigurationElement.CertificateThumbprint,
                    Id = windowsAzureSubscriptionConfigurationElement.Id,
                    SubscriptionId = windowsAzureSubscriptionConfigurationElement.GetWindowsAzureSubscriptionId()
                };
        }

        public WindowsAzurePackageConfiguration GetWindowsAzurePackageConfiguration(string windowsAzurePackageConfigurationId)
        {
            WindowsAzurePackageConfigurationElement windowsAzurePackageConfigurationElement = StealFocusForecastConfiguration.Instance.WindowsAzurePackages[windowsAzurePackageConfigurationId];
            return new WindowsAzurePackageConfiguration
                {
                    BlobName = windowsAzurePackageConfigurationElement.BlobName,
                    ContainerName = windowsAzurePackageConfigurationElement.ContainerName,
                    Id = windowsAzurePackageConfigurationElement.Id,
                    StorageAccountName = windowsAzurePackageConfigurationElement.StorageAccountName
                };
        }
        
        public WindowsAzureDeploymentDeleteConfiguration[] GetWindowsAzureDeploymentDeleteConfigurations()
        {
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzureDeploymentDeletes.Count);
            foreach (WindowsAzureDeploymentDeleteConfigurationElement windowsAzureDeploymentDeleteConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzureDeploymentDeletes)
            {
                WindowsAzureDeploymentDeleteConfiguration windowsAzureDeploymentDeleteConfiguration = new WindowsAzureDeploymentDeleteConfiguration();
                windowsAzureDeploymentDeleteConfiguration.Id = windowsAzureDeploymentDeleteConfigurationElement.Id;
                windowsAzureDeploymentDeleteConfiguration.PollingIntervalInMinutes = windowsAzureDeploymentDeleteConfigurationElement.PollingIntervalInMinutes;
                windowsAzureDeploymentDeleteConfiguration.ServiceName = windowsAzureDeploymentDeleteConfigurationElement.ServiceName;
                windowsAzureDeploymentDeleteConfiguration.SubscriptionConfigurationId = windowsAzureDeploymentDeleteConfigurationElement.SubscriptionConfigurationId;
                foreach (DeploymentSlotConfigurationElement deploymentSlotConfigurationElement in windowsAzureDeploymentDeleteConfigurationElement.DeploymentSlots)
                {
                    windowsAzureDeploymentDeleteConfiguration.DeploymentSlots.Add(deploymentSlotConfigurationElement.Name);
                }

                foreach (ScheduleConfiguration scheduleConfiguration in GetScheduleConfigurations(windowsAzureDeploymentDeleteConfigurationElement.Schedules))
                {
                    windowsAzureDeploymentDeleteConfiguration.Schedules.Add(scheduleConfiguration);
                }

                list.Add(windowsAzureDeploymentDeleteConfiguration);
            }

            return (WindowsAzureDeploymentDeleteConfiguration[])list.ToArray(typeof(WindowsAzureDeploymentDeleteConfiguration));
        }

        public WindowsAzureDeploymentCreateConfiguration[] GetWindowsAzureDeploymentCreateConfigurations()
        {
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzureDeploymentCreates.Count);
            foreach (WindowsAzureDeploymentCreateConfigurationElement windowsAzureDeploymentCreateConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzureDeploymentCreates)
            {
                WindowsAzureDeploymentCreateConfiguration windowsAzureDeploymentCreateConfiguration = new WindowsAzureDeploymentCreateConfiguration();
                windowsAzureDeploymentCreateConfiguration.DeploymentLabel = windowsAzureDeploymentCreateConfigurationElement.DeploymentLabel;
                windowsAzureDeploymentCreateConfiguration.DeploymentName = windowsAzureDeploymentCreateConfigurationElement.DeploymentName;
                windowsAzureDeploymentCreateConfiguration.DeploymentSlot = windowsAzureDeploymentCreateConfigurationElement.DeploymentSlot;
                windowsAzureDeploymentCreateConfiguration.Id = windowsAzureDeploymentCreateConfigurationElement.Id;
                windowsAzureDeploymentCreateConfiguration.PackageConfigurationFilePath = windowsAzureDeploymentCreateConfigurationElement.PackageConfigurationFilePath;
                windowsAzureDeploymentCreateConfiguration.PollingIntervalInMinutes = windowsAzureDeploymentCreateConfigurationElement.PollingIntervalInMinutes;
                windowsAzureDeploymentCreateConfiguration.ServiceName = windowsAzureDeploymentCreateConfigurationElement.ServiceName;
                windowsAzureDeploymentCreateConfiguration.StartDeployment = windowsAzureDeploymentCreateConfigurationElement.StartDeployment;
                windowsAzureDeploymentCreateConfiguration.SubscriptionConfigurationId = windowsAzureDeploymentCreateConfigurationElement.SubscriptionConfigurationId;
                windowsAzureDeploymentCreateConfiguration.TreatWarningsAsError = windowsAzureDeploymentCreateConfigurationElement.TreatWarningsAsError;
                windowsAzureDeploymentCreateConfiguration.WindowsAzurePackageId = windowsAzureDeploymentCreateConfigurationElement.WindowsAzurePackageId;
                foreach (ScheduleConfiguration scheduleConfiguration in GetScheduleConfigurations(windowsAzureDeploymentCreateConfigurationElement.Schedules))
                {
                    windowsAzureDeploymentCreateConfiguration.Schedules.Add(scheduleConfiguration);
                }

                list.Add(windowsAzureDeploymentCreateConfiguration);
            }

            return (WindowsAzureDeploymentCreateConfiguration[])list.ToArray(typeof(WindowsAzureDeploymentCreateConfiguration));
        }

        private static IEnumerable<ScheduleConfiguration> GetScheduleConfigurations(ScheduleConfigurationElementCollection scheduleConfigurationElementCollection)
        {
            ArrayList list = new ArrayList(scheduleConfigurationElementCollection.Count);
            foreach (ScheduleConfigurationElement scheduleConfigurationElement in scheduleConfigurationElementCollection)
            {
                ScheduleConfiguration scheduleConfiguration = new ScheduleConfiguration();
                scheduleConfiguration.Name = scheduleConfigurationElement.Name;
                foreach (DayConfigurationElement dayConfigurationElement in scheduleConfigurationElement.Days)
                {
                    DayConfiguration dayConfiguration = new DayConfiguration();
                    dayConfiguration.DayOfWeek = dayConfigurationElement.GetDayOfWeek();
                    dayConfiguration.EndTime = dayConfigurationElement.EndTime;
                    dayConfiguration.StartTime = dayConfigurationElement.StartTime;
                    scheduleConfiguration.Days.Add(dayConfiguration);
                }

                list.Add(scheduleConfiguration);
            }

            return (ScheduleConfiguration[])list.ToArray(typeof(ScheduleConfiguration));
        }
    }
}
