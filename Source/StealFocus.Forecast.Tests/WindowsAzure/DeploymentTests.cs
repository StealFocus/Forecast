namespace StealFocus.Forecast.Tests.WindowsAzure
{
    using System;
    using Forecast.WindowsAzure;
    using Forecast.WindowsAzure.Storage;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DeploymentTests
    {
        [TestMethod]
        [Ignore]
        public void TestCheckExists()
        {
            IDeployment deployment = new Deployment();
            bool result = deployment.CheckExists(
                WindowsAzureTests.SubscriptionId,
                WindowsAzureTests.CertificateThumbprint,
                "BeazleyTasks-WEuro-Sys",
                DeploymentSlot.Production);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [Ignore]
        public void TestDeleteRequest()
        {
            IDeployment deployment = new Deployment();
            deployment.DeleteRequest(WindowsAzureTests.SubscriptionId, WindowsAzureTests.CertificateThumbprint, "BeazleyTasks-WEuro-Sys", DeploymentSlot.Production);
        }

        [TestMethod]
        [Ignore]
        public void TestCreateRequest()
        {
            Uri packageUrl = Blob.GetUrl("bzytasksweurosys", "mydeployments", "20111207_015202_Beazley.Tasks.Azure.cspkg");
            IDeployment deployment = new Deployment();
            deployment.CreateRequest(
                WindowsAzureTests.SubscriptionId, 
                WindowsAzureTests.CertificateThumbprint, 
                "BeazleyTasks-WEuro-Sys", 
                DeploymentSlot.Production, 
                "Bad Deployment Name", 
                packageUrl,
                "DeploymentLabel",
                "ServiceConfiguration.Cloud-SysTest.cscfg", 
                true, 
                true);
        }
    }
}
