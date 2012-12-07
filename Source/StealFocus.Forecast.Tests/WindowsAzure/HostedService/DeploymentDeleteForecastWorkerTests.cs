namespace StealFocus.Forecast.Tests.WindowsAzure.HostedService
{
    using System;
    using System.Net;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using StealFocus.AzureExtensions.HostedService;
    using StealFocus.Forecast.WindowsAzure.HostedService;

    [TestClass]
    public class DeploymentDeleteForecastWorkerTests
    {
        private const string CertificateThumbprint = "0000000000000000000000000000000000000000";

        private const string ServiceName = "serviceName";

        private const string DeploymentSlot = "Production";

        private const string DeleteRequestId = "id";

        private readonly Guid subscriptionId = Guid.NewGuid();

        private readonly TimeSpan oneHour = new TimeSpan(1, 0, 0);

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Exists_And_Second_Call_Within_Polling_Interval()
        {
            MockRepository mockRepository = new MockRepository();

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
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
                .Expect(d => d.DeleteRequest(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(DeleteRequestId);
            mockOperation
                .Expect(o => o.StatusCheck(this.subscriptionId, CertificateThumbprint, DeleteRequestId))
                .Repeat.Once()
                .Return(operationResult);

            // Act
            mockRepository.ReplayAll();
            DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                mockDeployment, 
                mockOperation,
                this.subscriptionId, 
                CertificateThumbprint, 
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            deploymentDeleteForecastWorker.DoWork();
            deploymentDeleteForecastWorker.DoWork(); // Call DoWork twice to check the polling window works.

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

            // Set polling window to zero so the second call to "DoWork" is not within the first polling window.
            const int PollingIntervalInMinutes = 0;
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
                .Repeat.Twice()
                .Return(true);
            mockDeployment
                .Expect(d => d.DeleteRequest(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Twice()
                .Return(DeleteRequestId);
            mockOperation
                .Expect(o => o.StatusCheck(this.subscriptionId, CertificateThumbprint, DeleteRequestId))
                .Repeat.Twice()
                .Return(operationResult);

            // Act
            mockRepository.ReplayAll();
            DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            deploymentDeleteForecastWorker.DoWork();
            Thread.Sleep(10);
            deploymentDeleteForecastWorker.DoWork(); // Call DoWork twice to check the polling window works.

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Exists_And_Delete_Throws_An_Error()
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
                .Return(true);
            mockDeployment
                .Expect(d => d.DeleteRequest(this.subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Throw(new Exception("Error"));

            // Act
            mockRepository.ReplayAll();
            DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            deploymentDeleteForecastWorker.DoWork();

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
            DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            deploymentDeleteForecastWorker.DoWork();

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
                .Throw(new WebException("Error."));

            // Act
            mockRepository.ReplayAll();
            DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            deploymentDeleteForecastWorker.DoWork();

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
            DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            deploymentDeleteForecastWorker.DoWork();

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
            DeploymentDeleteForecastWorker deploymentDeleteForecastWorker = new DeploymentDeleteForecastWorker(
                mockDeployment,
                mockOperation,
                this.subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = notToday, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            deploymentDeleteForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }
    }
}
