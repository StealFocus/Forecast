namespace StealFocus.Forecast.Configuration
{
    using System.Collections;
    using System.Globalization;

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
                windowsAzureDeploymentDeleteConfiguration.PollingIntervalInMinutes = windowsAzureDeploymentDeleteConfigurationElement.PollingIntervalInMinutes;
                windowsAzureDeploymentDeleteConfiguration.ServiceName = windowsAzureDeploymentDeleteConfigurationElement.ServiceName;
                windowsAzureDeploymentDeleteConfiguration.SubscriptionConfigurationId = windowsAzureDeploymentDeleteConfigurationElement.SubscriptionConfigurationId;
                foreach (DeploymentSlotConfigurationElement deploymentSlotConfigurationElement in windowsAzureDeploymentDeleteConfigurationElement.DeploymentSlots)
                {
                    windowsAzureDeploymentDeleteConfiguration.DeploymentSlots.Add(deploymentSlotConfigurationElement.Name);
                }

                foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureDeploymentDeleteConfigurationElement.Schedules)
                {
                    ScheduleDefinitionConfiguration scheduleDefinitionConfiguration = GetScheduleDefinitionConfiguration(scheduleConfigurationElement.ScheduleDefinitionName);
                    windowsAzureDeploymentDeleteConfiguration.Schedules.Add(scheduleDefinitionConfiguration);
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
                windowsAzureDeploymentCreateConfiguration.PackageConfigurationFilePath = windowsAzureDeploymentCreateConfigurationElement.PackageConfigurationFilePath;
                windowsAzureDeploymentCreateConfiguration.PollingIntervalInMinutes = windowsAzureDeploymentCreateConfigurationElement.PollingIntervalInMinutes;
                windowsAzureDeploymentCreateConfiguration.ServiceName = windowsAzureDeploymentCreateConfigurationElement.ServiceName;
                windowsAzureDeploymentCreateConfiguration.StartDeployment = windowsAzureDeploymentCreateConfigurationElement.StartDeployment;
                windowsAzureDeploymentCreateConfiguration.SubscriptionConfigurationId = windowsAzureDeploymentCreateConfigurationElement.SubscriptionConfigurationId;
                windowsAzureDeploymentCreateConfiguration.TreatWarningsAsError = windowsAzureDeploymentCreateConfigurationElement.TreatWarningsAsError;
                windowsAzureDeploymentCreateConfiguration.WindowsAzurePackageId = windowsAzureDeploymentCreateConfigurationElement.WindowsAzurePackageId;
                foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureDeploymentCreateConfigurationElement.Schedules)
                {
                    ScheduleDefinitionConfiguration scheduleDefinitionConfiguration = GetScheduleDefinitionConfiguration(scheduleConfigurationElement.ScheduleDefinitionName);
                    windowsAzureDeploymentCreateConfiguration.Schedules.Add(scheduleDefinitionConfiguration);
                }

                list.Add(windowsAzureDeploymentCreateConfiguration);
            }

            return (WindowsAzureDeploymentCreateConfiguration[])list.ToArray(typeof(WindowsAzureDeploymentCreateConfiguration));
        }

        public WindowsAzureTableDeleteConfiguration[] GetWindowsAzureTableDeleteConfigurations()
        {
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzureTableDeletes.Count);
            foreach (WindowsAzureTableDeleteConfigurationElement windowsAzureTableDeleteConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzureTableDeletes)
            {
                WindowsAzureTableDeleteConfiguration windowsAzureTableDeleteConfiguration = new WindowsAzureTableDeleteConfiguration();
                windowsAzureTableDeleteConfiguration.PollingIntervalInMinutes = windowsAzureTableDeleteConfigurationElement.PollingIntervalInMinutes;
                windowsAzureTableDeleteConfiguration.StorageAccountKey = windowsAzureTableDeleteConfigurationElement.StorageAccountKey;
                windowsAzureTableDeleteConfiguration.StorageAccountName = windowsAzureTableDeleteConfigurationElement.StorageAccountName;
                windowsAzureTableDeleteConfiguration.TableName = windowsAzureTableDeleteConfigurationElement.TableName;
                foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureTableDeleteConfigurationElement.Schedules)
                {
                    ScheduleDefinitionConfiguration scheduleDefinitionConfiguration = GetScheduleDefinitionConfiguration(scheduleConfigurationElement.ScheduleDefinitionName);
                    windowsAzureTableDeleteConfiguration.Schedules.Add(scheduleDefinitionConfiguration);
                }

                list.Add(windowsAzureTableDeleteConfiguration);
            }

            return (WindowsAzureTableDeleteConfiguration[])list.ToArray(typeof(WindowsAzureTableDeleteConfiguration));
        }

        private static ScheduleDefinitionConfiguration GetScheduleDefinitionConfiguration(string scheduleDefinitionName)
        {
            foreach (ScheduleDefinitionConfigurationElement scheduleDefinitionConfigurationElement in StealFocusForecastConfiguration.Instance.ScheduleDefinitions)
            {
                if (scheduleDefinitionConfigurationElement.Name == scheduleDefinitionName)
                {
                    ScheduleDefinitionConfiguration scheduleDefinitionConfiguration = new ScheduleDefinitionConfiguration();
                    scheduleDefinitionConfiguration.Name = scheduleDefinitionConfigurationElement.Name;
                    foreach (DayConfigurationElement dayConfigurationElement in scheduleDefinitionConfigurationElement.Days)
                    {
                        DayConfiguration dayConfiguration = new DayConfiguration();
                        dayConfiguration.DayOfWeek = dayConfigurationElement.GetDayOfWeek();
                        dayConfiguration.EndTime = dayConfigurationElement.EndTime;
                        dayConfiguration.StartTime = dayConfigurationElement.StartTime;
                        scheduleDefinitionConfiguration.Days.Add(dayConfiguration);
                    }

                    return scheduleDefinitionConfiguration;
                }
            }

            string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The required schedule definition name of '{0}' was not found in the set of schedule definitions.", scheduleDefinitionName);
            throw new ForecastException(exceptionMessage);
        }
    }
}
