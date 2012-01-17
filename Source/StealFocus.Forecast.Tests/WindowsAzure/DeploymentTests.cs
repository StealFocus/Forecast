namespace StealFocus.Forecast.Tests.WindowsAzure
{
    using Forecast.WindowsAzure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DeploymentTests
    {
        [TestMethod]
        public void TestDelete()
        {
            Deployment.Delete();
        }
    }
}
