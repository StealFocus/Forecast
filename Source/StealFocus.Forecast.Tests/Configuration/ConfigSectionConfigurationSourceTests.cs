namespace StealFocus.Forecast.Tests.Configuration
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.Forecast.Configuration;

    [TestClass]
    public class ConfigSectionConfigurationSourceTests
    {
        [TestMethod]
        public void UnitTestGetWindowsAzureSubscriptionConfiguration()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            WindowsAzureSubscriptionConfiguration windowsAzureSubscriptionConfiguration = configSectionConfigurationSource.GetWindowsAzureSubscriptionConfiguration("myArbitraryAzureSubscriptionName");
            Assert.IsNotNull(windowsAzureSubscriptionConfiguration);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzurePackageConfiguration()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            WindowsAzurePackageConfiguration windowsAzurePackageConfiguration = configSectionConfigurationSource.GetWindowsAzurePackageConfiguration("myArbitraryPackageName");
            Assert.IsNotNull(windowsAzurePackageConfiguration);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureDeploymentCreateConfigurations()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            WindowsAzureDeploymentCreateConfiguration[] windowsAzureDeploymentCreateConfigurations = configSectionConfigurationSource.GetWindowsAzureDeploymentCreateConfigurations();
            Assert.AreEqual(1, windowsAzureDeploymentCreateConfigurations.Length);
            Assert.AreEqual(1, windowsAzureDeploymentCreateConfigurations[0].Schedules.Count);
            Assert.AreEqual(5, windowsAzureDeploymentCreateConfigurations[0].Schedules[0].Days.Count);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureDeploymentDeleteConfigurations()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            WindowsAzureDeploymentDeleteConfiguration[] windowsAzureDeploymentDeleteConfigurations = configSectionConfigurationSource.GetWindowsAzureDeploymentDeleteConfigurations();
            Assert.AreEqual(1, windowsAzureDeploymentDeleteConfigurations.Length);
            Assert.AreEqual(2, windowsAzureDeploymentDeleteConfigurations[0].DeploymentSlots.Count);
            Assert.AreEqual(3, windowsAzureDeploymentDeleteConfigurations[0].Schedules.Count);
            Assert.AreEqual(5, windowsAzureDeploymentDeleteConfigurations[0].Schedules[0].Days.Count);
            Assert.AreEqual(5, windowsAzureDeploymentDeleteConfigurations[0].Schedules[1].Days.Count);
            Assert.AreEqual(2, windowsAzureDeploymentDeleteConfigurations[0].Schedules[2].Days.Count);
        }

        [TestMethod]
        public void UnitTestGetWindowsAzureTableDeleteConfigurations()
        {
            ConfigSectionConfigurationSource configSectionConfigurationSource = new ConfigSectionConfigurationSource();
            WindowsAzureTableDeleteConfiguration[] windowsAzureTableDeleteConfigurations = configSectionConfigurationSource.GetWindowsAzureTableDeleteConfigurations();
            Assert.AreEqual(1, windowsAzureTableDeleteConfigurations.Length);
            Assert.AreEqual(2, windowsAzureTableDeleteConfigurations[0].TableNames.Count);
            Assert.AreEqual(3, windowsAzureTableDeleteConfigurations[0].Schedules.Count);
            Assert.AreEqual(5, windowsAzureTableDeleteConfigurations[0].Schedules[0].Days.Count);
            Assert.AreEqual(5, windowsAzureTableDeleteConfigurations[0].Schedules[1].Days.Count);
            Assert.AreEqual(2, windowsAzureTableDeleteConfigurations[0].Schedules[2].Days.Count);
        }
    }
}
