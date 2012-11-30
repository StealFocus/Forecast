namespace StealFocus.Forecast.WindowsAzure.StorageService
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using log4net;

    using StealFocus.AzureExtensions.StorageService;

    internal class TableDeleteForecastWorker : ForecastWorker
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object SyncRoot = new object();

        private readonly ITableService tableService;

        private readonly string storageAccountName;

        private readonly string[] tableNames;

        private readonly ScheduleDay[] scheduleDays;

        private readonly int pollingIntervalInMinutes;

        private DateTime lastTimeWeDidWork = DateTime.MinValue;

        public TableDeleteForecastWorker(
            ITableService tableService,
            string storageAccountName,
            string[] tableNames,
            ScheduleDay[] scheduleDays,
            int pollingIntervalInMinutes)
            : base(GetWorkerId(storageAccountName))
        {
            this.tableService = tableService;
            this.storageAccountName = storageAccountName;
            this.tableNames = tableNames;
            this.scheduleDays = scheduleDays;
            this.pollingIntervalInMinutes = pollingIntervalInMinutes;
        }

        public override void DoWork()
        {
            if (this.lastTimeWeDidWork.AddMinutes(this.pollingIntervalInMinutes) < DateTime.Now)
            {
                string doingWorkLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is doing work.", this.GetType().Name, this.Id);
                Logger.Debug(doingWorkLogMessage);
                bool nowIsInTheSchedule = DetermineIfNowIsInTheSchedule(Logger, this.GetType().Name, this.Id, this.scheduleDays);
                if (nowIsInTheSchedule)
                {
                    lock (SyncRoot)
                    {
                        foreach (string tableName in this.tableNames)
                        {
                            string deletingTableMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is sending a delete request for table name '{2}' under storage account name '{3}'.", this.GetType().Name, this.Id, tableName, this.storageAccountName);
                            Logger.Info(deletingTableMessage);
                            bool deleteTableSuccess = this.tableService.DeleteTable(tableName);
                            if (deleteTableSuccess)
                            {
                                string deleteTableSuccessMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' deleted table name '{2}' under storage account name '{3}'.", this.GetType().Name, this.Id, tableName, this.storageAccountName);
                                Logger.Info(deleteTableSuccessMessage);
                            }
                            else
                            {
                                string deleteTableFailedMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' failed to delete table name '{2}' under storage account name '{3}'. The table probably did not exist.", this.GetType().Name, this.Id, tableName, this.storageAccountName);
                                Logger.Info(deleteTableFailedMessage);
                            }
                        }
                    }
                }

                this.lastTimeWeDidWork = DateTime.Now;
            }
            else
            {
                string notDoingWorkLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is skipping doing work as we did work within the current polling window.", this.GetType().Name, this.Id);
                Logger.Debug(notDoingWorkLogMessage);
            }
        }

        private static string GetWorkerId(string storageAccountName)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}", storageAccountName);
        }
    }
}
