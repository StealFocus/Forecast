namespace StealFocus.Forecast.Tests.Configuration
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.Forecast.Configuration;
    using StealFocus.Forecast.WindowsAzure.HostedService;

    [TestClass]
    public class StealFocusForecastConfigurationTests
    {
        [TestMethod]
        public void UnitTestGetWhiteListForecastWorker()
        {
            WhiteListForecastWorker whiteListForecastWorker = StealFocusForecastConfiguration.GetWhiteListForecastWorker();
            Assert.IsNotNull(whiteListForecastWorker);
        }

        [TestMethod]
        public void UnitTestGetDeploymentDeleteForecastWorkers()
        {
            DeploymentDeleteForecastWorker[] deploymentDeleteForecastWorkers = StealFocusForecastConfiguration.GetDeploymentDeleteForecastWorkers();
            Assert.AreEqual(6, deploymentDeleteForecastWorkers.Length);
            foreach (DeploymentDeleteForecastWorker deploymentDeleteForecastWorker in deploymentDeleteForecastWorkers)
            {
                Assert.IsNotNull(deploymentDeleteForecastWorker);
            }
        }

        [TestMethod]
        public void UnitTestGetDeploymentCreateForecastWorkers()
        {
            DeploymentCreateForecastWorker[] deploymentCreateForecastWorkers = StealFocusForecastConfiguration.GetDeploymentCreateForecastWorkers();
            Assert.AreEqual(1, deploymentCreateForecastWorkers.Length);
            foreach (DeploymentCreateForecastWorker deploymentCreateForecastWorker in deploymentCreateForecastWorkers)
            {
                Assert.IsNotNull(deploymentCreateForecastWorker);
            }
        }

        [TestMethod]
        public void UnitTestGetScheduledHorizontalScaleForecastWorkers()
        {
            ScheduledHorizontalScaleForecastWorker[] scheduledHorizontalScaleForecastWorkers = StealFocusForecastConfiguration.GetScheduledHorizontalScaleForecastWorkers();
            Assert.AreEqual(4, scheduledHorizontalScaleForecastWorkers.Length);
            foreach (ScheduledHorizontalScaleForecastWorker scheduledHorizontalScaleForecastWorker in scheduledHorizontalScaleForecastWorkers)
            {
                Assert.IsNotNull(scheduledHorizontalScaleForecastWorker);
            }
        }
    }
}
