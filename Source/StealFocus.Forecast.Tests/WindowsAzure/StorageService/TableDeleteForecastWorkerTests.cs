namespace StealFocus.Forecast.Tests.WindowsAzure.StorageService
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using StealFocus.AzureExtensions.StorageService;
    using StealFocus.Forecast.WindowsAzure.StorageService;

    [TestClass]
    public class TableDeleteForecastWorkerTests
    {
        private readonly TimeSpan oneHour = new TimeSpan(1, 0, 0);

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time()
        {
            MockRepository mockRepository = new MockRepository();
            const string StorageAccountName = "storageAccountName";
            const string TableName = "tableName";

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Subtract(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            ITableService mockTableService = mockRepository.StrictMock<ITableService>();
            mockTableService
                .Expect(ts => ts.DeleteTable(TableName))
                .Repeat.Once()
                .Return(true);

            // Act
            mockRepository.ReplayAll();
            TableDeleteForecastWorker tableDeleteForecastWorker = new TableDeleteForecastWorker(
                mockTableService,
                StorageAccountName,
                TableName,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            tableDeleteForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_Not_In_The_Scheduled_Time()
        {
            MockRepository mockRepository = new MockRepository();
            const string StorageAccountName = "storageAccountName";
            const string TableName = "tableName";

            // Set start time to 1 hour before now.
            TimeSpan dailyStartTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);

            // Set end time to 1 hour after now.
            TimeSpan dailyEndTime = (DateTime.Now - DateTime.Today).Add(this.oneHour);
            const int PollingIntervalInMinutes = 60;

            // Arrange
            ITableService mockTableService = mockRepository.StrictMock<ITableService>();

            // Act
            mockRepository.ReplayAll();
            TableDeleteForecastWorker tableDeleteForecastWorker = new TableDeleteForecastWorker(
                mockTableService,
                StorageAccountName,
                TableName,
                new[] { new ScheduleDay { DayOfWeek = DateTime.Now.DayOfWeek, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            tableDeleteForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestDoWork_With_Now_In_The_Scheduled_Time_But_Not_On_A_Scheduled_Day()
        {
            MockRepository mockRepository = new MockRepository();
            const string StorageAccountName = "storageAccountName";
            const string TableName = "tableName";

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
            ITableService mockTableService = mockRepository.StrictMock<ITableService>();

            // Act
            mockRepository.ReplayAll();
            TableDeleteForecastWorker tableDeleteForecastWorker = new TableDeleteForecastWorker(
                mockTableService,
                StorageAccountName,
                TableName,
                new[] { new ScheduleDay { DayOfWeek = notToday, EndTime = dailyEndTime, StartTime = dailyStartTime } },
                PollingIntervalInMinutes);
            tableDeleteForecastWorker.DoWork();

            // Assert
            mockRepository.VerifyAll();
        }
    }
}
