namespace StealFocus.Forecast.WindowsAzure.StorageService
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Reflection;

    using log4net;

    using StealFocus.AzureExtensions.StorageService;

    internal class BlobContainerDeleteForecastWorker : ForecastWorker
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object SyncRoot = new object();

        private readonly IBlobService blobService;

        private readonly string storageAccountName;

        private readonly string[] containerNames;

        private readonly ScheduleDay[] scheduleDays;

        private readonly int pollingIntervalInMinutes;

        private DateTime lastTimeWeDidWork = DateTime.MinValue;

        public BlobContainerDeleteForecastWorker(
            IBlobService blobService,
            string storageAccountName,
            string[] containerNames,
            ScheduleDay[] scheduleDays,
            int pollingIntervalInMinutes)
            : base(GetWorkerId(storageAccountName))
        {
            this.blobService = blobService;
            this.storageAccountName = storageAccountName;
            this.containerNames = containerNames;
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
                        foreach (string containerName in this.containerNames)
                        {
                            string deletingContainerMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' is sending a delete request for container name '{2}' under storage account name '{3}'.", this.GetType().Name, this.Id, containerName, this.storageAccountName);
                            Logger.Info(deletingContainerMessage);
                            try
                            {
                                bool deleteContainerSuccess = this.blobService.DeleteContainer(containerName);
                                if (deleteContainerSuccess)
                                {
                                    string deleteContainerSuccessMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' deleted container name '{2}' under storage account name '{3}'.", this.GetType().Name, this.Id, containerName, this.storageAccountName);
                                    Logger.Info(deleteContainerSuccessMessage);
                                }
                                else
                                {
                                    string deleteContainerFailedMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' failed to delete container name '{2}' under storage account name '{3}'.", this.GetType().Name, this.Id, containerName, this.storageAccountName);
                                    Logger.Info(deleteContainerFailedMessage);
                                }
                            }
                            catch (WebException e)
                            {
                                HttpWebResponse httpWebResponse = e.Response as HttpWebResponse;
                                if (httpWebResponse != null &&
                                    e.Status == WebExceptionStatus.ProtocolError &&
                                    httpWebResponse.StatusCode == HttpStatusCode.NotFound)
                                {
                                    string containerDidNotExistMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' failed to delete container name '{2}' under storage account name '{3}' as the container did not exist.", this.GetType().Name, this.Id, containerName, this.storageAccountName);
                                    Logger.Error(containerDidNotExistMessage);
                                }
                                else
                                {
                                    string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' experienced an error deleting container name '{2}' under storage account name '{3}'. The operation will be retried after the next polling interval.", this.GetType().Name, this.Id, containerName, this.storageAccountName);
                                    Logger.Error(errorMessage, e);
                                }
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
