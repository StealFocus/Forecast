﻿namespace StealFocus.Forecast.Configuration
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;

    using StealFocus.AzureExtensions.HostedService;
    using StealFocus.AzureExtensions.StorageService;
    using StealFocus.Forecast.WindowsAzure.HostedService;
    using StealFocus.Forecast.WindowsAzure.StorageService;

    internal partial class StealFocusForecastConfiguration
    {
        internal static DeploymentDeleteForecastWorker[] GetDeploymentDeleteForecastWorkers()
        {
            IConfigurationSource configurationSource = GetConfigurationSource();
            ArrayList list = new ArrayList();
            foreach (WindowsAzureDeploymentDeleteConfiguration windowsAzureDeploymentDeleteConfiguration in configurationSource.GetWindowsAzureDeploymentDeleteConfigurations())
            {
                WindowsAzureSubscriptionConfiguration windowsAzureSubscriptionConfiguration = 
                    configurationSource.GetWindowsAzureSubscriptionConfiguration(windowsAzureDeploymentDeleteConfiguration.SubscriptionConfigurationId);
                foreach (string deploymentSlot in windowsAzureDeploymentDeleteConfiguration.DeploymentSlots)
                {
                    foreach (ScheduleDefinitionConfiguration scheduleDefinitionConfiguration in windowsAzureDeploymentDeleteConfiguration.Schedules)
                    {
                        ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfiguration(scheduleDefinitionConfiguration);
                        DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                            new Deployment(),
                            new Operation(),
                            windowsAzureSubscriptionConfiguration.SubscriptionId,
                            windowsAzureSubscriptionConfiguration.CertificateThumbprint,
                            windowsAzureDeploymentDeleteConfiguration.ServiceName,
                            deploymentSlot,
                            scheduleDays,
                            windowsAzureDeploymentDeleteConfiguration.PollingIntervalInMinutes);
                        list.Add(deploymentDeleteForecastWorker);
                    }
                }
            }

            return (DeploymentDeleteForecastWorker[])list.ToArray(typeof(DeploymentDeleteForecastWorker));
        }

        internal static DeploymentCreateForecastWorker[] GetDeploymentCreateForecastWorkers()
        {
            IConfigurationSource configurationSource = GetConfigurationSource();
            ArrayList list = new ArrayList();
            foreach (WindowsAzureDeploymentCreateConfiguration windowsAzureDeploymentCreateConfiguration in configurationSource.GetWindowsAzureDeploymentCreateConfigurations())
            {
                WindowsAzureSubscriptionConfiguration windowsAzureSubscriptionConfiguration = configurationSource.GetWindowsAzureSubscriptionConfiguration(windowsAzureDeploymentCreateConfiguration.SubscriptionConfigurationId);
                WindowsAzurePackageConfiguration windowsAzurePackageConfiguration = configurationSource.GetWindowsAzurePackageConfiguration(windowsAzureDeploymentCreateConfiguration.WindowsAzurePackageId);
                foreach (ScheduleDefinitionConfiguration scheduleDefinitionConfiguration in windowsAzureDeploymentCreateConfiguration.Schedules)
                {
                    ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfiguration(scheduleDefinitionConfiguration);
                    Uri packageUrl = Blob.GetUrl(windowsAzurePackageConfiguration.StorageAccountName, windowsAzurePackageConfiguration.ContainerName, windowsAzurePackageConfiguration.BlobName);
                    DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                        new Deployment(),
                        new Operation(),
                        windowsAzureSubscriptionConfiguration.SubscriptionId,
                        windowsAzureSubscriptionConfiguration.CertificateThumbprint,
                        windowsAzureDeploymentCreateConfiguration.ServiceName,
                        windowsAzureDeploymentCreateConfiguration.DeploymentSlot,
                        scheduleDays,
                        windowsAzureDeploymentCreateConfiguration.DeploymentName,
                        packageUrl,
                        windowsAzureDeploymentCreateConfiguration.DeploymentLabel,
                        windowsAzureDeploymentCreateConfiguration.PackageConfigurationFilePath,
                        windowsAzureDeploymentCreateConfiguration.StartDeployment,
                        windowsAzureDeploymentCreateConfiguration.TreatWarningsAsError,
                        windowsAzureDeploymentCreateConfiguration.PollingIntervalInMinutes);
                    list.Add(deploymentCreateForecastWorker);
                }
            }

            return (DeploymentCreateForecastWorker[])list.ToArray(typeof(DeploymentCreateForecastWorker));
        }

        internal static TableDeleteForecastWorker[] GetTableDeleteForecastWorkers()
        {
            IConfigurationSource configurationSource = GetConfigurationSource();
            ArrayList list = new ArrayList();
            foreach (WindowsAzureTableDeleteConfiguration windowsAzureTableDeleteConfiguration in configurationSource.GetWindowsAzureTableDeleteConfigurations())
            {
                TableService tableService = new TableService(windowsAzureTableDeleteConfiguration.StorageAccountName, windowsAzureTableDeleteConfiguration.StorageAccountKey);
                foreach (ScheduleDefinitionConfiguration scheduleDefinitionConfiguration in windowsAzureTableDeleteConfiguration.Schedules)
                {
                    ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfiguration(scheduleDefinitionConfiguration);
                    TableDeleteForecastWorker tableDeleteForecastWorker = new TableDeleteForecastWorker(
                        tableService,
                        windowsAzureTableDeleteConfiguration.StorageAccountName,
                        windowsAzureTableDeleteConfiguration.TableNames.ToArray(), 
                        scheduleDays,
                        windowsAzureTableDeleteConfiguration.PollingIntervalInMinutes);
                    list.Add(tableDeleteForecastWorker);
                }
            }

            return (TableDeleteForecastWorker[])list.ToArray(typeof(TableDeleteForecastWorker));
        }

        private static ScheduleDay[] GetScheduleDaysFromScheduleConfiguration(ScheduleDefinitionConfiguration scheduleConfiguration)
        {
            ScheduleDay[] scheduleDays = new ScheduleDay[scheduleConfiguration.Days.Count];
            for (int i = 0; i < scheduleConfiguration.Days.Count; i++)
            {
                scheduleDays[i] = new ScheduleDay
                    {
                        DayOfWeek = scheduleConfiguration.Days[i].DayOfWeek,
                        EndTime = scheduleConfiguration.Days[i].EndTime,
                        StartTime = scheduleConfiguration.Days[i].StartTime
                    };
            }

            return scheduleDays;
        }

        private static IConfigurationSource GetConfigurationSource()
        {
            StealFocusForecastConfiguration stealFocusForecastConfiguration = StealFocusForecastConfiguration.Instance;
            if (stealFocusForecastConfiguration == null)
            {
                throw new ForecastException("Could not find the StealFocus Forecast Configuration Section, was the configuration section defined properly?");
            }
            
            string configurationSourceTypeName = stealFocusForecastConfiguration.CustomConfigurationSourceType;
            if (string.IsNullOrEmpty(configurationSourceTypeName))
            {
                return new ConfigSectionConfigurationSource();
            }

            Type configurationSourceType = Type.GetType(configurationSourceTypeName);
            if (configurationSourceType == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "Could not load the type '{0}' specified the 'configurationSourceType' value in the configuration.", configurationSourceTypeName);
                throw new ForecastException(exceptionMessage);
            }

            object configurationSourceInstance = Activator.CreateInstance(configurationSourceType);
            try
            {
                return (IConfigurationSource)configurationSourceInstance;
            }
            catch (InvalidCastException e)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "Could not cast the '{0}' type to a '{1}' type, was the interface implemented?", configurationSourceType.FullName, typeof(IConfigurationSource).FullName);
                throw new ForecastException(exceptionMessage, e);
            }
        }
    }
}
