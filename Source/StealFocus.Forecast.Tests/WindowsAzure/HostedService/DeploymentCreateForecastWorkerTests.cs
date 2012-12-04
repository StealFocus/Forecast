namespace StealFocus.Forecast.Tests.WindowsAzure.HostedService
{
    using System;
    using System.Net;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using StealFocus.AzureExtensions.HostedService;
    using StealFocus.Forecast.WindowsAzure.HostedService;

    [TestClass]
    public class DeploymentCreateForecastWorkerTests
    {
        private readonly TimeSpan oneHour = new TimeSpan(1, 0, 0);

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Exists()
        {
            MockRepository mockRepository = new MockRepository();
            Guid subscriptionId = Guid.NewGuid();
            const string CertificateThumbprint = "0000000000000000000000000000000000000000";
            const string ServiceName = "serviceName";
            const string DeploymentSlot = "Production";
            const string RequestId = "id";
            const string DeploymentName = "deploymentName";
            Uri packageUrl = new Uri("http://my.url");
            const string Label = "deploymentLabel";
            const string ConfigurationFilePath = @"C:\PathTo\MyPackageConfiguration.cscfg";
            const bool StartDeployment = true;
            const bool TreatWarningsAsError = true;

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
                .Expect(d => d.CheckExists(subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(false);
            mockDeployment
                .Expect(d => d.CreateRequest(subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot, DeploymentName, packageUrl, Label, ConfigurationFilePath, StartDeployment, TreatWarningsAsError))
                .Repeat.Once()
                .Return(RequestId);
            mockOperation
                .Expect(o => o.StatusCheck(subscriptionId, CertificateThumbprint, RequestId))
                .Repeat.Once()
                .Return(operationResult);

            // Act
            mockRepository.ReplayAll();
            DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                mockDeployment, 
                mockOperation, 
                subscriptionId, 
                CertificateThumbprint, 
                ServiceName, 
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                DeploymentName,
                packageUrl,
                Label,
                ConfigurationFilePath,
                StartDeployment,
                TreatWarningsAsError,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Exists_And_Delete_Throws_An_Error()
        {
            MockRepository mockRepository = new MockRepository();
            Guid subscriptionId = Guid.NewGuid();
            const string CertificateThumbprint = "0000000000000000000000000000000000000000";
            const string ServiceName = "serviceName";
            const string DeploymentSlot = "Production";
            const string DeploymentName = "deploymentName";
            Uri packageUrl = new Uri("http://my.url");
            const string Label = "deploymentLabel";
            const string ConfigurationFilePath = @"C:\PathTo\MyPackageConfiguration.cscfg";
            const bool StartDeployment = true;
            const bool TreatWarningsAsError = true;

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();
            mockDeployment
                .Expect(d => d.CheckExists(subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(false);
            mockDeployment
                .Expect(d => d.CreateRequest(subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot, DeploymentName, packageUrl, Label, ConfigurationFilePath, StartDeployment, TreatWarningsAsError))
                .Repeat.Once()
                .Throw(new WebException("Error."));

            // Act
            mockRepository.ReplayAll();
            DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                mockDeployment,
                mockOperation,
                subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                DeploymentName,
                packageUrl,
                Label,
                ConfigurationFilePath,
                StartDeployment,
                TreatWarningsAsError,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Deployment_Does_Not_Exist()
        {
            MockRepository mockRepository = new MockRepository();
            Guid subscriptionId = Guid.NewGuid();
            const string CertificateThumbprint = "0000000000000000000000000000000000000000";
            const string ServiceName = "serviceName";
            const string DeploymentSlot = "Production";
            const string DeploymentName = "deploymentName";
            Uri packageUrl = new Uri("http://my.url");
            const string Label = "deploymentLabel";
            const string ConfigurationFilePath = @"C:\PathTo\MyPackageConfiguration.cscfg";
            const bool StartDeployment = true;
            const bool TreatWarningsAsError = true;

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();
            mockDeployment
                .Expect(d => d.CheckExists(subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Return(true);

            // Act
            mockRepository.ReplayAll();
            DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                mockDeployment,
                mockOperation,
                subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                DeploymentName,
                packageUrl,
                Label,
                ConfigurationFilePath,
                StartDeployment,
                TreatWarningsAsError,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_And_Check_Exists_Throws_Error()
        {
            MockRepository mockRepository = new MockRepository();
            Guid subscriptionId = Guid.NewGuid();
            const string CertificateThumbprint = "0000000000000000000000000000000000000000";
            const string ServiceName = "serviceName";
            const string DeploymentSlot = "Production";
            const string DeploymentName = "deploymentName";
            Uri packageUrl = new Uri("http://my.url");
            const string Label = "deploymentLabel";
            const string ConfigurationFilePath = @"C:\PathTo\MyPackageConfiguration.cscfg";
            const bool StartDeployment = true;
            const bool TreatWarningsAsError = true;

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IDeployment mockDeployment = mockRepository.StrictMock<IDeployment>();
            IOperation mockOperation = mockRepository.StrictMock<IOperation>();
            mockDeployment
                .Expect(d => d.CheckExists(subscriptionId, CertificateThumbprint, ServiceName, DeploymentSlot))
                .Repeat.Once()
                .Throw(new WebException("Error."));

            // Act
            mockRepository.ReplayAll();
            DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                mockDeployment,
                mockOperation,
                subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                DeploymentName,
                packageUrl,
                Label,
                ConfigurationFilePath,
                StartDeployment,
                TreatWarningsAsError,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_Not_In_The_Scheduled_Time()
        {
            MockRepository mockRepository = new MockRepository();
            Guid subscriptionId = Guid.NewGuid();
            const string CertificateThumbprint = "0000000000000000000000000000000000000000";
            const string ServiceName = "serviceName";
            const string DeploymentSlot = "Production";
            const string DeploymentName = "deploymentName";
            Uri packageUrl = new Uri("http://my.url");
            const string Label = "deploymentLabel";
            const string ConfigurationFilePath = @"C:\PathTo\MyPackageConfiguration.cscfg";
            const bool StartDeployment = true;
            const bool TreatWarningsAsError = true;

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
            DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                mockDeployment,
                mockOperation,
                subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                DeploymentName,
                packageUrl,
                Label,
                ConfigurationFilePath,
                StartDeployment,
                TreatWarningsAsError,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_But_Not_On_A_Scheduled_Day()
        {
            MockRepository mockRepository = new MockRepository();
            Guid subscriptionId = Guid.NewGuid();
            const string CertificateThumbprint = "0000000000000000000000000000000000000000";
            const string ServiceName = "serviceName";
            const string DeploymentSlot = "Production";
            const string DeploymentName = "deploymentName";
            Uri packageUrl = new Uri("http://my.url");
            const string Label = "deploymentLabel";
            const string ConfigurationFilePath = @"C:\PathTo\MyPackageConfiguration.cscfg";
            const bool StartDeployment = true;
            const bool TreatWarningsAsError = true;

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
            DeploymentCreateForecastWorker deploymentCreateForecastWorker = new DeploymentCreateForecastWorker(
                mockDeployment,
                mockOperation,
                subscriptionId,
                CertificateThumbprint,
                ServiceName,
                DeploymentSlot,
                new[] { new ScheduleDay { DayOfWeek = notToday, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                DeploymentName,
                packageUrl,
                Label,
                ConfigurationFilePath,
                StartDeployment,
                TreatWarningsAsError,
                PollingIntervalInMinutes);
            deploymentCreateForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }
    }
}
