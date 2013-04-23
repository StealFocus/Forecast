namespace StealFocus.Forecast.Tests.WindowsAzure.HostedService
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using StealFocus.AzureExtensions;
    using StealFocus.AzureExtensions.HostedService;
    using StealFocus.Forecast.Configuration;
    using StealFocus.Forecast.Configuration.WindowsAzure;
    using StealFocus.Forecast.Configuration.WindowsAzure.HostedService;
    using StealFocus.Forecast.WindowsAzure.HostedService;

    [TestClass]
    public class WhiteListForecastWorkerTests
    {
        private const string CertificateThumbprint = "certificateThumbprint";

        private static readonly Guid SubscriptionId = Guid.NewGuid();

        [TestMethod]
        public void UnitTestDoWork_With_One_Role_Having_Instance_Count_Too_High_And_Second_Role_Having_Instance_Size_Too_Large()
        {
            MockRepository mockRepository = new MockRepository();
            WhiteListService whiteListService = new WhiteListService { Name = "wls1" };
            WhiteListRole whiteListRole = new WhiteListRole { Name = "wlr1", MaxInstanceSize = InstanceSize.ExtraSmall, MaxInstanceCount = 1 };
            whiteListService.Roles.Add(whiteListRole);
            WhiteListService[] whiteListServices = new[] { whiteListService };

            // Arrange
            ISubscription mockSubscription = mockRepository.StrictMock<ISubscription>();
            mockSubscription
                .Expect(s => s.ListHostedServices())
                .Repeat.Once()
                .Return(new[] { "bls1", "wls1" });
            mockSubscription
                .Expect(s => s.SubscriptionId)
                .Repeat.Any()
                .Return(SubscriptionId);
            mockSubscription
                .Expect(s => s.CertificateThumbprint)
                .Repeat.Any()
                .Return(CertificateThumbprint);
            ISubscription[] subscriptions = new[] { mockSubscription };
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();

            // Delete the black listed service from Staging and Production
            SetupDeleteRequestWithStatusCheck(mockDeployment, mockOperation, "bls1", DeploymentSlot.Staging, "deleteRequest1");
            SetupDeleteRequestWithStatusCheck(mockDeployment, mockOperation, "bls1", DeploymentSlot.Production, "deleteRequest2");

            // Get the instance size of White List Role 1 from Staging, all okay.
            mockDeployment
                .Expect(d => d.GetInstanceSize(SubscriptionId, CertificateThumbprint, "wls1", DeploymentSlot.Staging, "wlr1"))
                .Repeat.Once()
                .Return(InstanceSize.ExtraSmall.ToString());

            // Get the instance count of White List Role 1 from Staging, horizontally scale as a result.
            mockDeployment
                .Expect(d => d.GetInstanceCount(SubscriptionId, CertificateThumbprint, "wls1", DeploymentSlot.Staging, "wlr1"))
                .Repeat.Once()
                .Return(2);
            SetupHorizontallyScaleWithStatusCheck(mockDeployment, mockOperation, "wls1", DeploymentSlot.Staging, new[] { new HorizontalScale { RoleName = "wlr1", InstanceCount = 1 } }, "scaleRequest1");

            // Get the instance size of the White List Role 1 from Production, delete as a result.
            mockDeployment
                .Expect(d => d.GetInstanceSize(SubscriptionId, CertificateThumbprint, "wls1", DeploymentSlot.Production, "wlr1"))
                .Repeat.Once()
                .Return(InstanceSize.ExtraLarge.ToString());
            SetupDeleteRequestWithStatusCheck(mockDeployment, mockOperation, "wls1", DeploymentSlot.Production, "deleteRequest3");

            // Act
            mockRepository.ReplayAll();
            WhiteListForecastWorker whiteListForecastWorker = new WhiteListForecastWorker(subscriptions, mockDeployment, mockOperation, whiteListServices, 1);
            whiteListForecastWorker.DoWork();
            whiteListForecastWorker.DoWork(); // Call twice to test the polling interval (nothing should be invoked the second time).

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_No_Explicit_Roles_Configured_For_White_List_Service()
        {
            MockRepository mockRepository = new MockRepository();
            WhiteListService whiteListService = new WhiteListService { Name = "wls1" };
            WhiteListService[] whiteListServices = new[] { whiteListService };

            // Arrange
            ISubscription mockSubscription = mockRepository.StrictMock<ISubscription>();
            mockSubscription
                .Expect(s => s.ListHostedServices())
                .Repeat.Once()
                .Return(new[] { "wls1" });
            mockSubscription
                .Expect(s => s.SubscriptionId)
                .Repeat.Any()
                .Return(SubscriptionId);
            mockSubscription
                .Expect(s => s.CertificateThumbprint)
                .Repeat.Any()
                .Return(CertificateThumbprint);
            ISubscription[] subscriptions = new[] { mockSubscription };
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();

            // Act
            // There are no roles configured for the white list service, therefore there should be no checks made against 
            // the service, we just accept however it is deployed for instance count or instance size.
            mockRepository.ReplayAll();
            WhiteListForecastWorker whiteListForecastWorker = new WhiteListForecastWorker(subscriptions, mockDeployment, mockOperation, whiteListServices, 1);
            whiteListForecastWorker.DoWork();
            whiteListForecastWorker.DoWork(); // Call twice to test the polling interval (nothing should be invoked the second time).

            // Assert
            mockRepository.VerifyAll();
        }

        private static void SetupDeleteRequestWithStatusCheck(IDeployment mockDeployment, IOperation mockOperation, string serviceName, string deploymentSlot, string requestId)
        {
            mockDeployment
                .Expect(d => d.CheckExists(SubscriptionId, CertificateThumbprint, serviceName, deploymentSlot))
                .Repeat.Once()
                .Return(true);
            mockDeployment
                .Expect(d => d.DeleteRequest(SubscriptionId, CertificateThumbprint, serviceName, deploymentSlot))
                .Repeat.Once()
                .Return(requestId);
            mockOperation
                .Expect(o => o.StatusCheck(SubscriptionId, CertificateThumbprint, requestId))
                .Repeat.Once()
                .Return(new OperationResult { Status = OperationStatus.Succeeded });
        }

        private static void SetupHorizontallyScaleWithStatusCheck(IDeployment mockDeployment, IOperation mockOperation, string serviceName, string deploymentSlot, HorizontalScale[] horizontalScales, string requestId)
        {
            mockDeployment
                .Expect(d => d.CheckExists(SubscriptionId, CertificateThumbprint, serviceName, deploymentSlot))
                .Repeat.Once()
                .Return(true);
            mockDeployment
                .Expect(d => d.HorizontallyScale(SubscriptionId, CertificateThumbprint, serviceName, deploymentSlot, horizontalScales, true, Mode.Auto))
                .Repeat.Once()
                .Return(requestId);
            mockOperation
                .Expect(o => o.StatusCheck(SubscriptionId, CertificateThumbprint, requestId))
                .Repeat.Once()
                .Return(new OperationResult { Status = OperationStatus.Succeeded });
        }
    }
}
