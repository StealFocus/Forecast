namespace StealFocus.Forecast.Configuration
{
    using System.Collections;
    using System.Globalization;

    using StealFocus.Forecast.Configuration.WindowsAzure;

    internal class ConfigSectionConfigurationSource : IConfigurationSource
    {
        public SubscriptionConfiguration GetWindowsAzureSubscriptionConfiguration(string windowsAzureSubscriptionConfigurationId)
        {
            WindowsAzureSubscriptionConfigurationElement windowsAzureSubscriptionConfigurationElement = StealFocusForecastConfiguration.Instance.WindowsAzure.Subscriptions[windowsAzureSubscriptionConfigurationId];
            return new SubscriptionConfiguration
                {
                    CertificateThumbprint = windowsAzureSubscriptionConfigurationElement.CertificateThumbprint,
                    Id = windowsAzureSubscriptionConfigurationElement.Id,
                    SubscriptionId = windowsAzureSubscriptionConfigurationElement.GetWindowsAzureSubscriptionId()
                };
        }

        public StorageAccountConfiguration GetWindowsAzureStorageAccountConfiguration(string windowsAzureStorageAccountName)
        {
            WindowsAzureStorageAccountConfigurationElement windowsAzureStorageAccountConfigurationElement = StealFocusForecastConfiguration.Instance.WindowsAzure.StorageAccounts[windowsAzureStorageAccountName];
            return new StorageAccountConfiguration
                {
                    StorageAccountName = windowsAzureStorageAccountConfigurationElement.storageAccountName,
                    StorageAccountKey = windowsAzureStorageAccountConfigurationElement.storageAccountKey
                };
        }

        public PackageConfiguration GetWindowsAzurePackageConfiguration(string windowsAzurePackageConfigurationId)
        {
            WindowsAzurePackageConfigurationElement windowsAzurePackageConfigurationElement = StealFocusForecastConfiguration.Instance.WindowsAzure.Packages[windowsAzurePackageConfigurationId];
            return new PackageConfiguration
                {
                    BlobName = windowsAzurePackageConfigurationElement.BlobName,
                    ContainerName = windowsAzurePackageConfigurationElement.ContainerName,
                    Id = windowsAzurePackageConfigurationElement.Id,
                    StorageAccountName = windowsAzurePackageConfigurationElement.StorageAccountName
                };
        }

        public DeploymentDeleteConfiguration[] GetWindowsAzureDeploymentDeleteConfigurations()
        {
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzure.DeploymentDeletes.Count);
            foreach (WindowsAzureDeploymentDeleteConfigurationElement windowsAzureDeploymentDeleteConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.DeploymentDeletes)
            {
                DeploymentDeleteConfiguration deploymentDeleteConfiguration = new DeploymentDeleteConfiguration();
                deploymentDeleteConfiguration.PollingIntervalInMinutes = windowsAzureDeploymentDeleteConfigurationElement.PollingIntervalInMinutes;
                deploymentDeleteConfiguration.ServiceName = windowsAzureDeploymentDeleteConfigurationElement.ServiceName;
                deploymentDeleteConfiguration.SubscriptionConfigurationId = windowsAzureDeploymentDeleteConfigurationElement.SubscriptionConfigurationId;
                foreach (DeploymentSlotConfigurationElement deploymentSlotConfigurationElement in windowsAzureDeploymentDeleteConfigurationElement.DeploymentSlots)
                {
                    deploymentDeleteConfiguration.DeploymentSlots.Add(deploymentSlotConfigurationElement.Name);
                }

                foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureDeploymentDeleteConfigurationElement.Schedules)
                {
                    ScheduleDefinitionConfiguration scheduleDefinitionConfiguration = GetScheduleDefinitionConfiguration(scheduleConfigurationElement.ScheduleDefinitionName);
                    deploymentDeleteConfiguration.Schedules.Add(scheduleDefinitionConfiguration);
                }

                list.Add(deploymentDeleteConfiguration);
            }

            return (DeploymentDeleteConfiguration[])list.ToArray(typeof(DeploymentDeleteConfiguration));
        }

        public DeploymentCreateConfiguration[] GetWindowsAzureDeploymentCreateConfigurations()
        {
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzure.DeploymentCreates.Count);
            foreach (WindowsAzureDeploymentCreateConfigurationElement windowsAzureDeploymentCreateConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.DeploymentCreates)
            {
                DeploymentCreateConfiguration deploymentCreateConfiguration = new DeploymentCreateConfiguration();
                deploymentCreateConfiguration.DeploymentLabel = windowsAzureDeploymentCreateConfigurationElement.DeploymentLabel;
                deploymentCreateConfiguration.DeploymentName = windowsAzureDeploymentCreateConfigurationElement.DeploymentName;
                deploymentCreateConfiguration.DeploymentSlot = windowsAzureDeploymentCreateConfigurationElement.DeploymentSlot;
                deploymentCreateConfiguration.PackageConfigurationFilePath = windowsAzureDeploymentCreateConfigurationElement.PackageConfigurationFilePath;
                deploymentCreateConfiguration.PollingIntervalInMinutes = windowsAzureDeploymentCreateConfigurationElement.PollingIntervalInMinutes;
                deploymentCreateConfiguration.ServiceName = windowsAzureDeploymentCreateConfigurationElement.ServiceName;
                deploymentCreateConfiguration.StartDeployment = windowsAzureDeploymentCreateConfigurationElement.StartDeployment;
                deploymentCreateConfiguration.SubscriptionConfigurationId = windowsAzureDeploymentCreateConfigurationElement.SubscriptionConfigurationId;
                deploymentCreateConfiguration.TreatWarningsAsError = windowsAzureDeploymentCreateConfigurationElement.TreatWarningsAsError;
                deploymentCreateConfiguration.WindowsAzurePackageId = windowsAzureDeploymentCreateConfigurationElement.WindowsAzurePackageId;
                foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureDeploymentCreateConfigurationElement.Schedules)
                {
                    ScheduleDefinitionConfiguration scheduleDefinitionConfiguration = GetScheduleDefinitionConfiguration(scheduleConfigurationElement.ScheduleDefinitionName);
                    deploymentCreateConfiguration.Schedules.Add(scheduleDefinitionConfiguration);
                }

                list.Add(deploymentCreateConfiguration);
            }

            return (DeploymentCreateConfiguration[])list.ToArray(typeof(DeploymentCreateConfiguration));
        }

        public TableDeleteConfiguration[] GetWindowsAzureTableDeleteConfigurations()
        {
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzure.TableDeletes.Count);
            foreach (WindowsAzureTableDeleteConfigurationElement windowsAzureTableDeleteConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.TableDeletes)
            {
                TableDeleteConfiguration tableDeleteConfiguration = new TableDeleteConfiguration();
                tableDeleteConfiguration.PollingIntervalInMinutes = windowsAzureTableDeleteConfigurationElement.PollingIntervalInMinutes;
                tableDeleteConfiguration.StorageAccountName = windowsAzureTableDeleteConfigurationElement.StorageAccountName;
                StorageAccountConfiguration windowsAzureStorageAccountConfiguration = this.GetWindowsAzureStorageAccountConfiguration(windowsAzureTableDeleteConfigurationElement.StorageAccountName);
                tableDeleteConfiguration.StorageAccountKey = windowsAzureStorageAccountConfiguration.StorageAccountKey;
                foreach (StorageTableConfigurationElement storageTableConfigurationElement in windowsAzureTableDeleteConfigurationElement.StorageTables)
                {
                    tableDeleteConfiguration.TableNames.Add(storageTableConfigurationElement.tableName);
                }

                foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureTableDeleteConfigurationElement.Schedules)
                {
                    ScheduleDefinitionConfiguration scheduleDefinitionConfiguration = GetScheduleDefinitionConfiguration(scheduleConfigurationElement.ScheduleDefinitionName);
                    tableDeleteConfiguration.Schedules.Add(scheduleDefinitionConfiguration);
                }

                list.Add(tableDeleteConfiguration);
            }

            return (TableDeleteConfiguration[])list.ToArray(typeof(TableDeleteConfiguration));
        }

        public BlobContainerDeleteConfiguration[] GetWindowsAzureBlobContainerDeleteConfigurations()
        {
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzure.BlobContainerDeletes.Count);
            foreach (WindowsAzureBlobContainerDeleteConfigurationElement windowsAzureBlobContainerDeleteConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.BlobContainerDeletes)
            {
                BlobContainerDeleteConfiguration blobContainerDeleteConfiguration = new BlobContainerDeleteConfiguration();
                blobContainerDeleteConfiguration.PollingIntervalInMinutes = windowsAzureBlobContainerDeleteConfigurationElement.PollingIntervalInMinutes;
                blobContainerDeleteConfiguration.StorageAccountName = windowsAzureBlobContainerDeleteConfigurationElement.StorageAccountName;
                StorageAccountConfiguration windowsAzureStorageAccountConfiguration = this.GetWindowsAzureStorageAccountConfiguration(windowsAzureBlobContainerDeleteConfigurationElement.StorageAccountName);
                blobContainerDeleteConfiguration.StorageAccountKey = windowsAzureStorageAccountConfiguration.StorageAccountKey;
                foreach (BlobContainerConfigurationElement blobContainerConfigurationElement in windowsAzureBlobContainerDeleteConfigurationElement.BlobContainers)
                {
                    blobContainerDeleteConfiguration.BlobContainerNames.Add(blobContainerConfigurationElement.blobContainerName);
                }

                foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureBlobContainerDeleteConfigurationElement.Schedules)
                {
                    ScheduleDefinitionConfiguration scheduleDefinitionConfiguration = GetScheduleDefinitionConfiguration(scheduleConfigurationElement.ScheduleDefinitionName);
                    blobContainerDeleteConfiguration.Schedules.Add(scheduleDefinitionConfiguration);
                }

                list.Add(blobContainerDeleteConfiguration);
            }

            return (BlobContainerDeleteConfiguration[])list.ToArray(typeof(BlobContainerDeleteConfiguration));
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
