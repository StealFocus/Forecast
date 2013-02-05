namespace StealFocus.Forecast.Tests.Configuration
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.Forecast.Configuration;
    using StealFocus.Forecast.Configuration.WindowsAzure;
    using StealFocus.Forecast.Configuration.WindowsAzure.HostedService;
    using StealFocus.Forecast.Configuration.WindowsAzure.StorageService;
    using StealFocus.Forecast.WindowsAzure.HostedService;

    [TestClass]
    public class ConfigSectionConfigurationSourceTests
    {
        [TestMethod]
        public void UnitTestGetAllSubscriptionConfigurations()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            SubscriptionConfiguration[] allSubscriptionConfigurations = configSectionConfigurationSource.GetAllWindowsAzureSubscriptionConfigurations();
            Assert.AreEqual(1, allSubscriptionConfigurations.Length);
        }

        [TestMethod]
        public void UnitTestGetWhiteListConfiguration()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            WhiteListConfiguration whiteListConfiguration = configSectionConfigurationSource.GetWindowsAzureHostedServiceWhiteListConfiguration();
            Assert.AreEqual(true, whiteListConfiguration.IncludeDeploymentCreateServices);
            Assert.AreEqual(true, whiteListConfiguration.IncludeDeploymentDeleteServices);
            Assert.AreEqual(true, whiteListConfiguration.IncludeHorizontalScaleServices);
            Assert.AreEqual(60, whiteListConfiguration.PollingIntervalInMinutes);
            Assert.AreEqual(2, whiteListConfiguration.Services.Count);

            // Test 1st service
            Assert.AreEqual("myAzureServiceName1", whiteListConfiguration.Services[0].Name);
            Assert.AreEqual(2, whiteListConfiguration.Services[0].Roles.Count);
            Assert.AreEqual(1, whiteListConfiguration.Services[0].Roles[0].MaxInstanceCount);
            Assert.AreEqual(InstanceSize.Medium, whiteListConfiguration.Services[0].Roles[0].MaxInstanceSize);
            Assert.AreEqual(2, whiteListConfiguration.Services[0].Roles[1].MaxInstanceCount);
            Assert.AreEqual(InstanceSize.ExtraSmall, whiteListConfiguration.Services[0].Roles[1].MaxInstanceSize);

            // Test 2nd service
            Assert.AreEqual("myAzureServiceName2", whiteListConfiguration.Services[1].Name);
            Assert.AreEqual(1, whiteListConfiguration.Services[1].Roles.Count);
            Assert.IsNull(whiteListConfiguration.Services[1].Roles[0].MaxInstanceCount);
            Assert.IsNull(whiteListConfiguration.Services[1].Roles[0].MaxInstanceSize);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureSubscriptionConfiguration()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            SubscriptionConfiguration subscriptionConfiguration = configSectionConfigurationSource.GetWindowsAzureSubscriptionConfiguration("myArbitraryAzureSubscriptionName");
            Assert.IsNotNull(subscriptionConfiguration);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureStorageAccountConfiguration()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            StorageAccountConfiguration storageAccountConfiguration = configSectionConfigurationSource.GetWindowsAzureStorageAccountConfiguration("myStorageAccountName");
            Assert.IsNotNull(storageAccountConfiguration);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzurePackageConfiguration()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            PackageConfiguration packageConfiguration = configSectionConfigurationSource.GetWindowsAzurePackageConfiguration("myArbitraryPackageName");
            Assert.IsNotNull(packageConfiguration);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureDeploymentCreateConfigurations()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            DeploymentCreateConfiguration[] deploymentCreateConfigurations = configSectionConfigurationSource.GetWindowsAzureDeploymentCreateConfigurations();
            Assert.AreEqual(1, deploymentCreateConfigurations.Length);
            Assert.AreEqual(1, deploymentCreateConfigurations[0].Schedules.Count);
            Assert.AreEqual(5, deploymentCreateConfigurations[0].Schedules[0].Days.Count);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureDeploymentDeleteConfigurations()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            DeploymentDeleteConfiguration[] deploymentDeleteConfigurations = configSectionConfigurationSource.GetWindowsAzureDeploymentDeleteConfigurations();
            Assert.AreEqual(1, deploymentDeleteConfigurations.Length);
            Assert.AreEqual(2, deploymentDeleteConfigurations[0].DeploymentSlots.Count);
            Assert.AreEqual(3, deploymentDeleteConfigurations[0].Schedules.Count);
            Assert.AreEqual(5, deploymentDeleteConfigurations[0].Schedules[0].Days.Count);
            Assert.AreEqual(5, deploymentDeleteConfigurations[0].Schedules[1].Days.Count);
            Assert.AreEqual(2, deploymentDeleteConfigurations[0].Schedules[2].Days.Count);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureScheduledHorizontalScaleConfigurations()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            ScheduledHorizontalScaleConfiguration[] scheduledHorizontalScaleConfigurations = configSectionConfigurationSource.GetWindowsAzureScheduledHorizontalScaleConfigurations();
            Assert.AreEqual(2, scheduledHorizontalScaleConfigurations.Length);
            Assert.AreEqual(1, scheduledHorizontalScaleConfigurations[1].HorizontalScales.Count);
            Assert.AreEqual(3, scheduledHorizontalScaleConfigurations[1].Schedules.Count);
            Assert.AreEqual(5, scheduledHorizontalScaleConfigurations[1].Schedules[0].Days.Count);
            Assert.AreEqual(5, scheduledHorizontalScaleConfigurations[1].Schedules[1].Days.Count);
            Assert.AreEqual(2, scheduledHorizontalScaleConfigurations[1].Schedules[2].Days.Count);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureTableDeleteConfigurations()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            TableDeleteConfiguration[] tableDeleteConfigurations = configSectionConfigurationSource.GetWindowsAzureTableDeleteConfigurations();
            Assert.AreEqual(1, tableDeleteConfigurations.Length);
            Assert.AreEqual(2, tableDeleteConfigurations[0].TableNames.Count);
            Assert.AreEqual(3, tableDeleteConfigurations[0].Schedules.Count);
            Assert.AreEqual(5, tableDeleteConfigurations[0].Schedules[0].Days.Count);
            Assert.AreEqual(5, tableDeleteConfigurations[0].Schedules[1].Days.Count);
            Assert.AreEqual(2, tableDeleteConfigurations[0].Schedules[2].Days.Count);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureBlobContainerDeleteConfigurations()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            BlobContainerDeleteConfiguration[] blobContainerDeleteConfigurations = configSectionConfigurationSource.GetWindowsAzureBlobContainerDeleteConfigurations();
            Assert.AreEqual(1, blobContainerDeleteConfigurations.Length);
            Assert.AreEqual(2, blobContainerDeleteConfigurations[0].BlobContainerNames.Count);
            Assert.AreEqual(3, blobContainerDeleteConfigurations[0].Schedules.Count);
            Assert.AreEqual(5, blobContainerDeleteConfigurations[0].Schedules[0].Days.Count);
            Assert.AreEqual(5, blobContainerDeleteConfigurations[0].Schedules[1].Days.Count);
            Assert.AreEqual(2, blobContainerDeleteConfigurations[0].Schedules[2].Days.Count);
        }
    }
}
