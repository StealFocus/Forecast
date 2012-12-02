namespace StealFocus.Forecast.Tests.WindowsAzure.StorageService
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using StealFocus.AzureExtensions.StorageService;
    using StealFocus.Forecast.WindowsAzure.StorageService;

    [TestClass]
    public class BlobContainerDeleteForecastWorkerTests
    {
        private readonly TimeSpan oneHour = new TimeSpan(1, 0, 0);

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time()
        {
            MockRepository mockRepository = new MockRepository();
            const string StorageAccountName = "storageAccountName";
            const string BlobContainerName1 = "blobContainerName1";
            const string BlobContainerName2 = "blobContainerName2";

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IBlobService mockBlobService = mockRepository.StrictMock<IBlobService>();
            mockBlobService
                .Expect(ts => ts.DeleteContainer(string.Empty))
                .IgnoreArguments()
                .Repeat.Twice()
                .Return(true);

            // Act
            mockRepository.ReplayAll();
            BlobContainerDeleteForecastWorker blobContainerDeleteForecastWorker = new BlobContainerDeleteForecastWorker(
                mockBlobService,
                StorageAccountName,
                new[] { BlobContainerName1, BlobContainerName2 },
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            blobContainerDeleteForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_Not_In_The_Scheduled_Time()
        {
            MockRepository mockRepository = new MockRepository();
            const string StorageAccountName = "storageAccountName";
            const string BlobContainerName = "blobContainerName";

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            IBlobService mockBlobService = mockRepository.StrictMock<IBlobService>();

            // Act
            mockRepository.ReplayAll();
            BlobContainerDeleteForecastWorker blobContainerDeleteForecastWorker = new BlobContainerDeleteForecastWorker(
                mockBlobService,
                StorageAccountName,
                new[] { BlobContainerName },
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            blobContainerDeleteForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_But_Not_On_A_Scheduled_Day()
        {
            MockRepository mockRepository = new MockRepository();
            const string StorageAccountName = "storageAccountName";
            const string BlobContainerName = "blobContainerName";

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
            IBlobService mockBlobService = mockRepository.StrictMock<IBlobService>();

            // Act
            mockRepository.ReplayAll();
            BlobContainerDeleteForecastWorker blobContainerDeleteForecastWorker = new BlobContainerDeleteForecastWorker(
                mockBlobService,
                StorageAccountName,
                new[] { BlobContainerName },
                new[] { new ScheduleDay { DayOfWeek = notToday, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            blobContainerDeleteForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }
    }
}
