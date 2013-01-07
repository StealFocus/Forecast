namespace StealFocus.Forecast.Tests.WindowsAzure.HostedService
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Xml.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using StealFocus.AzureExtensions.HostedService;
    using StealFocus.Forecast.Configuration.WindowsAzure.HostedService;
    using StealFocus.Forecast.WindowsAzure.HostedService;

    [TestClass]
    public class ScheduledHorizontalScaleForecastWorkerTests
    {
        private const string CertificateThumbprint = "0000000000000000000000000000000000000000";

        private const string ServiceName = "serviceName";

        private const string DeploymentSlot = "Production";

        private const string RequestId = "id";

        private const string RoleName = "myRoleName";

        private const bool TreatWarningsAsError = true;

        private const string Mode = "Auto";

        private readonly Guid subscriptionId = Guid.NewGuid();

        private readonly TimeSpan oneHour = new TimeSpan(1, 0, 0);

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Exists_And_Requires_Scaling_Out()
        {
            MockRepository mockRepository = new MockRepository();

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int RequiredInstanceCount = 2;
            const int ConfiguredInstanceCount = 1;
            const int PollingIntervalInMinutes = 60;
            OperationResult operationResult = new OperationResult
                {
                    Code = "Test",
                    HttpStatusCode = HttpStatusCode.OK,
                    Id = Guid.NewGuid(),
                    Message = string.Empty,
                    Status = OperationStatus.Succeeded
                };

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();
            mockDeployment
                .Expect(d => d.CheckExists(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(true);
            mockDeployment
                .Expect(d => d.GetConfiguration(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(GetServiceConfiguration(RoleName, ConfiguredInstanceCount));
            mockDeployment
                .Expect(d => d.ChangeConfiguration(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot, null, TreatWarningsAsError, Mode))
                .Repeat.Once()
                .IgnoreArguments()
                .Return(RequestId);
            mockOperation
                .Expect(o => o.StatusCheck(this.subscriptionId, CertificateThumbprint, RequestId))
                .Repeat.Once()
                .Return(operationResult);

            // Act
            mockRepository.ReplayAll();
            ScheduledHorizontalScaleForecastWorker deploymentCreateForecastWorker = new ScheduledHorizontalScaleForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                RoleName,
                RequiredInstanceCount,
                TreatWarningsAsError,
                Mode,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();
            deploymentCreateForecastWorker.DoWork(); // Call DoWork twice to check the polling window works.

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Exists_And_Second_Call_Within_Polling_Interval()
        {
            MockRepository mockRepository = new MockRepository();

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int RequiredInstanceCount = 2;
            const int ConfiguredInstanceCount = RequiredInstanceCount;
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();
            mockDeployment
                .Expect(d => d.CheckExists(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(true);
            mockDeployment
                .Expect(d => d.GetConfiguration(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(GetServiceConfiguration(RoleName, ConfiguredInstanceCount));

            // Act
            mockRepository.ReplayAll();
            ScheduledHorizontalScaleForecastWorker deploymentCreateForecastWorker = new ScheduledHorizontalScaleForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                RoleName,
                RequiredInstanceCount,
                TreatWarningsAsError,
                Mode,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();
            deploymentCreateForecastWorker.DoWork(); // Call DoWork twice to check the polling window works.

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Exists_And_Second_Call_Not_Within_Polling_Interval()
        {
            MockRepository mockRepository = new MockRepository();

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int RequiredInstanceCount = 2;
            const int ConfiguredInstanceCount = RequiredInstanceCount;

            // Set polling window to zero so the second call to "DoWork" is not within the first polling window.
            const int PollingIntervalInMinutes = 0;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();
            mockDeployment
                .Expect(d => d.CheckExists(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Twice()
                .Return(true);
            mockDeployment
                .Expect(d => d.GetConfiguration(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Twice()
                .Return(GetServiceConfiguration(RoleName, ConfiguredInstanceCount));

            // Act
            mockRepository.ReplayAll();
            ScheduledHorizontalScaleForecastWorker deploymentCreateForecastWorker = new ScheduledHorizontalScaleForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                RoleName,
                RequiredInstanceCount,
                TreatWarningsAsError,
                Mode,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();
            Thread.Sleep(10);
            deploymentCreateForecastWorker.DoWork(); // Call DoWork twice to check the polling window works.

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Exists_And_GetConfiguration_Throws_An_Error()
        {
            MockRepository mockRepository = new MockRepository();

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);
            const int RequiredInstanceCount = 2;

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();
            mockDeployment
                .Expect(d => d.CheckExists(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(true);
            mockDeployment
                .Expect(d => d.GetConfiguration(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Throw(new WebException("Error."));

            // Act
            mockRepository.ReplayAll();
            ScheduledHorizontalScaleForecastWorker deploymentCreateForecastWorker = new ScheduledHorizontalScaleForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                RoleName,
                RequiredInstanceCount,
                TreatWarningsAsError,
                Mode,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Does_Not_Exist()
        {
            MockRepository mockRepository = new MockRepository();

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();
            mockDeployment
                .Expect(d => d.CheckExists(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(false);

            // Act
            mockRepository.ReplayAll();
            ScheduledHorizontalScaleForecastWorker deploymentCreateForecastWorker = new ScheduledHorizontalScaleForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                RoleName,
                1,
                TreatWarningsAsError,
                Mode,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Check_Exists_Throws_Error()
        {
            MockRepository mockRepository = new MockRepository();

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();
            mockDeployment
                .Expect(d => d.CheckExists(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Throw(new Exception("Error."));

            // Act
            mockRepository.ReplayAll();
            ScheduledHorizontalScaleForecastWorker deploymentCreateForecastWorker = new ScheduledHorizontalScaleForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                RoleName,
                1,
                TreatWarningsAsError,
                Mode,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_Not_In_The_Scheduled_Time()
        {
            MockRepository mockRepository = new MockRepository();

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();

            // Act
            mockRepository.ReplayAll();
            ScheduledHorizontalScaleForecastWorker deploymentCreateForecastWorker = new ScheduledHorizontalScaleForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                RoleName,
                1,
                TreatWarningsAsError,
                Mode,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_But_Not_On_A_Scheduled_Day()
        {
            MockRepository mockRepository = new MockRepository();

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);

            // Get a day that is not today.
            DayOfWeek notToday = DayOfWeek.Sunday;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                notToday = DayOfWeek.Monday;
            }

            const int PollingIntervalInMinutes = 60;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();

            // Act
            mockRepository.ReplayAll();
            ScheduledHorizontalScaleForecastWorker deploymentCreateForecastWorker = new ScheduledHorizontalScaleForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = notToday, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                RoleName,
                1,
                TreatWarningsAsError,
                Mode,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        private static XDocument GetServiceConfiguration(string roleName, int instanceCount)
        {
            /*
            <ServiceConfiguration
             serviceName=""
             osFamily="1"
             osVersion="*"
             xmlns="http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration">
              <Role name="Beazley.Marketing.Profiles.Endpoint">
                <ConfigurationSettings>
                  <Setting ... />
                </ConfigurationSettings>
                <Instances count="1" />
                <Certificates />
              </Role>
            </ServiceConfiguration>
            */
            XNamespace serviceConfigurationNamespace = XmlNamespace.ServiceHosting200810ServiceConfiguration;
            XDocument serviceConfigurationXml = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(
                    serviceConfigurationNamespace + "ServiceConfiguration",
                    new XElement(
                        serviceConfigurationNamespace + "Role", 
                        new XAttribute("name", roleName),
                        new XElement(
                            serviceConfigurationNamespace + "Instances",
                            new XAttribute("count", instanceCount)))));
            return serviceConfigurationXml;
        }
    }
}
