namespace StealFocus.Forecast.Configuration
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;

    using StealFocus.AzureExtensions;
    using StealFocus.AzureExtensions.HostedService;
    using StealFocus.AzureExtensions.StorageService;
    using StealFocus.Forecast.Configuration.WindowsAzure;
    using StealFocus.Forecast.Configuration.WindowsAzure.HostedService;
    using StealFocus.Forecast.Configuration.WindowsAzure.StorageService;
    using StealFocus.Forecast.WindowsAzure.HostedService;
    using StealFocus.Forecast.WindowsAzure.StorageService;

    internal partial class StealFocusForecastConfiguration
    {
        internal static WhiteListForecastWorker GetWhiteListForecastWorker()
        {
            IConfigurationSource configurationSource = GetConfigurationSource();
            WhiteListConfiguration whiteListConfiguration = configurationSource.GetWindowsAzureHostedServiceWhiteListConfiguration();
            if (whiteListConfiguration != null)
            {
                SubscriptionConfiguration[] subscriptionConfigurations = configurationSource.GetAllWindowsAzureSubscriptionConfigurations();
                ISubscription[] subscriptions = new ISubscription[subscriptionConfigurations.Length];
                for (int i = 0; i < subscriptionConfigurations.Length; i++)
                {
                    subscriptions[i] = subscriptionConfigurations[i].Convert();
                }

                if (whiteListConfiguration.IncludeDeploymentCreateServices)
                {
                    foreach (DeploymentCreateConfiguration deploymentCreateConfiguration in configurationSource.GetWindowsAzureDeploymentCreateConfigurations())
                    {
                        WhiteListService whiteListService = new WhiteListService { Name = deploymentCreateConfiguration.ServiceName };
                        whiteListConfiguration.Services.Add(whiteListService);
                    }
                }

                if (whiteListConfiguration.IncludeDeploymentDeleteServices)
                {
                    foreach (DeploymentDeleteConfiguration deploymentDeleteConfiguration in configurationSource.GetWindowsAzureDeploymentDeleteConfigurations())
                    {
                        WhiteListService whiteListService = new WhiteListService { Name = deploymentDeleteConfiguration.ServiceName };
                        whiteListConfiguration.Services.Add(whiteListService);
                    }
                }

                if (whiteListConfiguration.IncludeHorizontalScaleServices)
                {
                    foreach (ScheduledHorizontalScaleConfiguration windowsAzureScheduledHorizontalScaleConfiguration in configurationSource.GetWindowsAzureScheduledHorizontalScaleConfigurations())
                    {
                        WhiteListService whiteListService = new WhiteListService { Name = windowsAzureScheduledHorizontalScaleConfiguration.ServiceName };
                        whiteListConfiguration.Services.Add(whiteListService);
                    }
                }

                WhiteListForecastWorker whiteListForecastWorker = new WhiteListForecastWorker(
                    subscriptions,
                    new Deployment(),
                    new Operation(),
                    whiteListConfiguration.Services.ToArray(),
                    whiteListConfiguration.PollingIntervalInMinutes);
                return whiteListForecastWorker;
            }

            return null;
        }

        internal static DeploymentDeleteForecastWorker[] GetDeploymentDeleteForecastWorkers()
        {
            IConfigurationSource configurationSource = GetConfigurationSource();
            ArrayList list = new ArrayList();
            foreach (DeploymentDeleteConfiguration deploymentDeleteConfiguration in configurationSource.GetWindowsAzureDeploymentDeleteConfigurations())
            {
                SubscriptionConfiguration subscriptionConfiguration = configurationSource.GetWindowsAzureSubscriptionConfiguration(deploymentDeleteConfiguration.SubscriptionConfigurationId);
                foreach (string deploymentSlot in deploymentDeleteConfiguration.DeploymentSlots)
                {
                    foreach (ScheduleDefinitionConfiguration scheduleDefinitionConfiguration in deploymentDeleteConfiguration.Schedules)
                    {
                        ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfiguration(scheduleDefinitionConfiguration);
                        DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                            new Deployment(),
                            new Operation(),
                            subscriptionConfiguration.SubscriptionId,
                            subscriptionConfiguration.CertificateThumbprint,
                            deploymentDeleteConfiguration.ServiceName,
                            deploymentSlot,
                            scheduleDays,
                            deploymentDeleteConfiguration.PollingIntervalInMinutes);
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
            foreach (DeploymentCreateConfiguration deploymentCreateConfiguration in configurationSource.GetWindowsAzureDeploymentCreateConfigurations())
            {
                SubscriptionConfiguration subscriptionConfiguration = configurationSource.GetWindowsAzureSubscriptionConfiguration(deploymentCreateConfiguration.SubscriptionConfigurationId);
                PackageConfiguration packageConfiguration = configurationSource.GetWindowsAzurePackageConfiguration(deploymentCreateConfiguration.WindowsAzurePackageId);
                foreach (ScheduleDefinitionConfiguration scheduleDefinitionConfiguration in deploymentCreateConfiguration.Schedules)
                {
                    ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfiguration(scheduleDefinitionConfiguration);
                    Uri packageUrl = Blob.GetUrl(packageConfiguration.StorageAccountName, packageConfiguration.ContainerName, packageConfiguration.BlobName);
                    DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                        new Deployment(),
                        new Operation(),
                        subscriptionConfiguration.SubscriptionId,
                        subscriptionConfiguration.CertificateThumbprint,
                        deploymentCreateConfiguration.ServiceName,
                        deploymentCreateConfiguration.DeploymentSlot,
                        scheduleDays,
                        deploymentCreateConfiguration.DeploymentName,
                        packageUrl,
                        deploymentCreateConfiguration.DeploymentLabel,
                        deploymentCreateConfiguration.PackageConfigurationFilePath,
                        deploymentCreateConfiguration.StartDeployment,
                        deploymentCreateConfiguration.TreatWarningsAsError,
                        deploymentCreateConfiguration.PollingIntervalInMinutes);
                    list.Add(deploymentCreateForecastWorker);
                }
            }

            return (DeploymentCreateForecastWorker[])list.ToArray(typeof(DeploymentCreateForecastWorker));
        }

        internal static ScheduledHorizontalScaleForecastWorker[] GetScheduledHorizontalScaleForecastWorkers()
        {
            IConfigurationSource configurationSource = GetConfigurationSource();
            ArrayList list = new ArrayList();
            foreach (ScheduledHorizontalScaleConfiguration scheduledHorizontalScaleConfiguration in configurationSource.GetWindowsAzureScheduledHorizontalScaleConfigurations())
            {
                SubscriptionConfiguration subscriptionConfiguration = configurationSource.GetWindowsAzureSubscriptionConfiguration(scheduledHorizontalScaleConfiguration.SubscriptionConfigurationId);
                foreach (ScheduleDefinitionConfiguration scheduleDefinitionConfiguration in scheduledHorizontalScaleConfiguration.Schedules)
                {
                    HorizontalScale[] horizontalScales = GetHorizontalScalesFromHorizontalScaleConfiguration(scheduledHorizontalScaleConfiguration.HorizontalScales.ToArray());
                    ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfiguration(scheduleDefinitionConfiguration);
                    ScheduledHorizontalScaleForecastWorker scheduledHorizontalScaleForecastWorker = new ScheduledHorizontalScaleForecastWorker(
                        new Deployment(),
                        new Operation(),
                        subscriptionConfiguration.SubscriptionId,
                        subscriptionConfiguration.CertificateThumbprint,
                        scheduledHorizontalScaleConfiguration.ServiceName,
                        scheduledHorizontalScaleConfiguration.DeploymentSlot,
                        horizontalScales,
                        scheduleDays,
                        scheduledHorizontalScaleConfiguration.TreatWarningsAsError,
                        scheduledHorizontalScaleConfiguration.Mode,
                        scheduledHorizontalScaleConfiguration.PollingIntervalInMinutes);
                    list.Add(scheduledHorizontalScaleForecastWorker);
                }
            }

            return (ScheduledHorizontalScaleForecastWorker[])list.ToArray(typeof(ScheduledHorizontalScaleForecastWorker));
        }

        internal static TableDeleteForecastWorker[] GetTableDeleteForecastWorkers()
        {
            IConfigurationSource configurationSource = GetConfigurationSource();
            ArrayList list = new ArrayList();
            foreach (TableDeleteConfiguration tableDeleteConfiguration in configurationSource.GetWindowsAzureTableDeleteConfigurations())
            {
                TableService tableService = new TableService(tableDeleteConfiguration.StorageAccountName, tableDeleteConfiguration.StorageAccountKey);
                foreach (ScheduleDefinitionConfiguration scheduleDefinitionConfiguration in tableDeleteConfiguration.Schedules)
                {
                    ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfiguration(scheduleDefinitionConfiguration);
                    TableDeleteForecastWorker tableDeleteForecastWorker = new TableDeleteForecastWorker(
                        tableService,
                        tableDeleteConfiguration.StorageAccountName,
                        tableDeleteConfiguration.TableNames.ToArray(), 
                        scheduleDays,
                        tableDeleteConfiguration.PollingIntervalInMinutes);
                    list.Add(tableDeleteForecastWorker);
                }
            }

            return (TableDeleteForecastWorker[])list.ToArray(typeof(TableDeleteForecastWorker));
        }

        internal static BlobContainerDeleteForecastWorker[] GetBlobContainerDeleteForecastWorkers()
        {
            IConfigurationSource configurationSource = GetConfigurationSource();
            ArrayList list = new ArrayList();
            foreach (BlobContainerDeleteConfiguration blobContainerDeleteConfiguration in configurationSource.GetWindowsAzureBlobContainerDeleteConfigurations())
            {
                BlobService blobService = new BlobService(blobContainerDeleteConfiguration.StorageAccountName, blobContainerDeleteConfiguration.StorageAccountKey);
                foreach (ScheduleDefinitionConfiguration scheduleDefinitionConfiguration in blobContainerDeleteConfiguration.Schedules)
                {
                    ScheduleDay[] scheduleDays = GetScheduleDaysFromScheduleConfiguration(scheduleDefinitionConfiguration);
                    BlobContainerDeleteForecastWorker blobContainerDeleteForecastWorker = new BlobContainerDeleteForecastWorker(
                        blobService,
                        blobContainerDeleteConfiguration.StorageAccountName,
                        blobContainerDeleteConfiguration.BlobContainerNames.ToArray(),
                        scheduleDays,
                        blobContainerDeleteConfiguration.PollingIntervalInMinutes);
                    list.Add(blobContainerDeleteForecastWorker);
                }
            }

            return (BlobContainerDeleteForecastWorker[])list.ToArray(typeof(BlobContainerDeleteForecastWorker));
        }

        private static HorizontalScale[] GetHorizontalScalesFromHorizontalScaleConfiguration(HorizontalScaleConfiguration[] horizontalScaleConfigurations)
        {
            HorizontalScale[] horizontalScales = new HorizontalScale[horizontalScaleConfigurations.Length];
            for (int i = 0; i < horizontalScaleConfigurations.Length; i++)
            {
                horizontalScales[i] = new HorizontalScale
                    {
                        RoleName = horizontalScaleConfigurations[i].RoleName,
                        InstanceCount = horizontalScaleConfigurations[i].InstanceCount
                    };
            }

            return horizontalScales;
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
