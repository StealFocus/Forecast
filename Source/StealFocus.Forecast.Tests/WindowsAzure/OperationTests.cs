namespace StealFocus.Forecast.Tests.WindowsAzure
{
    using System;
    using Forecast.WindowsAzure;
    using Forecast.WindowsAzure.Storage;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OperationTests
    {
        [TestMethod]
        [Ignore]
        public void TestCheckStatus1()
        {
            IDeployment deployment = new Deployment();
            string deleteRequestId = deployment.DeleteRequest(WindowsAzureTests.SubscriptionId, WindowsAzureTests.CertificateThumbprint, "BeazleyTasks-WEuro-Sys", DeploymentSlot.Production);
            Operation.StatusCheck(WindowsAzureTests.SubscriptionId, WindowsAzureTests.CertificateThumbprint, deleteRequestId);
        }

        [TestMethod]
        [Ignore]
        public void TestCheckStatus2()
        {
            Uri packageUrl = Blob.GetUrl("bzytasksweurosys", "mydeployments", "20111207_015202_Beazley.Tasks.Azure.cspkg");
            IDeployment deployment = new Deployment();
            string createRequestId = deployment.CreateRequest(
                WindowsAzureTests.SubscriptionId,
                WindowsAzureTests.CertificateThumbprint,
                "BeazleyTasks-WEuro-Sys",
                DeploymentSlot.Production,
                "DeploymentName",
                packageUrl,
                "DeploymentLabel",
                "ServiceConfiguration.Cloud-SysTest.cscfg",
                true,
                true);
            Operation.StatusCheck(WindowsAzureTests.SubscriptionId, WindowsAzureTests.CertificateThumbprint, createRequestId);
            System.Threading.Thread.Sleep(5000);
            Operation.StatusCheck(WindowsAzureTests.SubscriptionId, WindowsAzureTests.CertificateThumbprint, createRequestId);
        }
    }
}
