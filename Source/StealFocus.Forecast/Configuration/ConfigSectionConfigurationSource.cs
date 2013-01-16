﻿namespace StealFocus.Forecast.Configuration
{
    using System.Collections;
    using System.Globalization;

    using StealFocus.Forecast.Configuration.WindowsAzure;
    using StealFocus.Forecast.Configuration.WindowsAzure.HostedService;
    using StealFocus.Forecast.Configuration.WindowsAzure.StorageService;

    internal class ConfigSectionConfigurationSource : IConfigurationSource
    {
        public SubscriptionConfiguration[] GetAllWindowsAzureSubscriptionConfigurations()
        {
            SubscriptionConfiguration[] subscriptionConfigurations = new SubscriptionConfiguration[StealFocusForecastConfiguration.Instance.WindowsAzure.Subscriptions.Count];
            for (int i = 0; i < StealFocusForecastConfiguration.Instance.WindowsAzure.Subscriptions.Count; i++)
            {
                subscriptionConfigurations[i] = this.GetWindowsAzureSubscriptionConfiguration(StealFocusForecastConfiguration.Instance.WindowsAzure.Subscriptions[i].Id);
            }

            return subscriptionConfigurations;
        }

        public WhiteListConfiguration GetWindowsAzureHostedServiceWhiteListConfiguration()
        {
            WhiteListConfiguration whiteListConfiguration = new WhiteListConfiguration();
            whiteListConfiguration.IncludeDeploymentCreateServices = StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.WhiteList.IncludeDeploymentCreateServices;
            whiteListConfiguration.IncludeDeploymentDeleteServices = StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.WhiteList.IncludeDeploymentDeleteServices;
            whiteListConfiguration.IncludeHorizontalScaleServices = StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.WhiteList.IncludeHorizontalScaleServices;
            whiteListConfiguration.PollingIntervalInMinutes = StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.WhiteList.PollingIntervalInMinutes;
            foreach (ServiceConfigurationElement serviceConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.WhiteList)
            {
                whiteListConfiguration.ServiceNames.Add(serviceConfigurationElement.Name);
            }

            return whiteListConfiguration;
        }

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
            WindowsAzureStorageAccountConfigurationElement windowsAzureStorageAccountConfigurationElement = StealFocusForecastConfiguration.Instance.WindowsAzure.StorageService.StorageAccounts[windowsAzureStorageAccountName];
            return new StorageAccountConfiguration
                {
                    StorageAccountName = windowsAzureStorageAccountConfigurationElement.storageAccountName,
                    StorageAccountKey = windowsAzureStorageAccountConfigurationElement.storageAccountKey
                };
        }

        public PackageConfiguration GetWindowsAzurePackageConfiguration(string windowsAzurePackageConfigurationId)
        {
            WindowsAzurePackageConfigurationElement windowsAzurePackageConfigurationElement = StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.Packages[windowsAzurePackageConfigurationId];
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
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.DeploymentDeletes.Count);
            foreach (WindowsAzureDeploymentDeleteConfigurationElement windowsAzureDeploymentDeleteConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.DeploymentDeletes)
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
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.DeploymentCreates.Count);
            foreach (WindowsAzureDeploymentCreateConfigurationElement windowsAzureDeploymentCreateConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.DeploymentCreates)
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

        public ScheduledHorizontalScaleConfiguration[] GetWindowsAzureScheduledHorizontalScaleConfigurations()
        {
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.ScheduledHorizontalScales.Count);
            foreach (WindowsAzureScheduledHorizontalScaleConfigurationElement windowsAzureScheduledHorizontalScaleConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.HostedService.ScheduledHorizontalScales)
            {
                ScheduledHorizontalScaleConfiguration scheduledHorizontalScaleConfiguration = new ScheduledHorizontalScaleConfiguration();
                scheduledHorizontalScaleConfiguration.DeploymentSlot = windowsAzureScheduledHorizontalScaleConfigurationElement.DeploymentSlot;
                scheduledHorizontalScaleConfiguration.Mode = windowsAzureScheduledHorizontalScaleConfigurationElement.Mode;
                scheduledHorizontalScaleConfiguration.PollingIntervalInMinutes = windowsAzureScheduledHorizontalScaleConfigurationElement.PollingIntervalInMinutes;
                scheduledHorizontalScaleConfiguration.ServiceName = windowsAzureScheduledHorizontalScaleConfigurationElement.ServiceName;
                scheduledHorizontalScaleConfiguration.SubscriptionConfigurationId = windowsAzureScheduledHorizontalScaleConfigurationElement.SubscriptionConfigurationId;
                scheduledHorizontalScaleConfiguration.TreatWarningsAsError = windowsAzureScheduledHorizontalScaleConfigurationElement.TreatWarningsAsError;
                foreach (HorizontalScaleConfigurationElement horizontalScaleConfigurationElement in windowsAzureScheduledHorizontalScaleConfigurationElement.HorizontalScales)
                {
                    HorizontalScaleConfiguration horizontalScaleConfiguration = new HorizontalScaleConfiguration
                        {
                            RoleName = horizontalScaleConfigurationElement.roleName,
                            InstanceCount = horizontalScaleConfigurationElement.instanceCount
                        };
                    scheduledHorizontalScaleConfiguration.HorizontalScales.Add(horizontalScaleConfiguration);
                }
                
                foreach (ScheduleConfigurationElement scheduleConfigurationElement in windowsAzureScheduledHorizontalScaleConfigurationElement.Schedules)
                {
                    ScheduleDefinitionConfiguration scheduleDefinitionConfiguration = GetScheduleDefinitionConfiguration(scheduleConfigurationElement.ScheduleDefinitionName);
                    scheduledHorizontalScaleConfiguration.Schedules.Add(scheduleDefinitionConfiguration);
                }

                list.Add(scheduledHorizontalScaleConfiguration);
            }

            return (ScheduledHorizontalScaleConfiguration[])list.ToArray(typeof(ScheduledHorizontalScaleConfiguration));
        }

        public TableDeleteConfiguration[] GetWindowsAzureTableDeleteConfigurations()
        {
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzure.StorageService.TableDeletes.Count);
            foreach (WindowsAzureTableDeleteConfigurationElement windowsAzureTableDeleteConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.StorageService.TableDeletes)
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
            ArrayList list = new ArrayList(StealFocusForecastConfiguration.Instance.WindowsAzure.StorageService.BlobContainerDeletes.Count);
            foreach (WindowsAzureBlobContainerDeleteConfigurationElement windowsAzureBlobContainerDeleteConfigurationElement in StealFocusForecastConfiguration.Instance.WindowsAzure.StorageService.BlobContainerDeletes)
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
