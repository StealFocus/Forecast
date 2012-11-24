namespace StealFocus.Forecast.Tests.Configuration
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.Forecast.Configuration;
    using StealFocus.Forecast.WindowsAzure;

    [TestClass]
    public class StealFocusForecastConfigurationTests
    {
        [TestMethod]
        public void UnitTestGetDeploymentDeleteForecastWorkers()
        {
            DeploymentDeleteForecastWorker[] deploymentDeleteForecastWorkers = StealFocusForecastConfiguration.Instance.GetDeploymentDeleteForecastWorkers();
            Assert.AreEqual(4, deploymentDeleteForecastWorkers.Length);
            foreach (DeploymentDeleteForecastWorker deploymentDeleteForecastWorker in deploymentDeleteForecastWorkers)
            {
                Assert.IsNotNull(deploymentDeleteForecastWorker);
            }
        }
    }
}
