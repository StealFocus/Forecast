namespace StealFocus.Forecast
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;

    using log4net;

    using StealFocus.AzureExtensions.HostedService;

    internal abstract class ForecastWorker
    {
        private const int FiveSecondsInMilliseconds = 5000;

        private const int TenSeccondsInMilliseconds = 10000;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Holds the thread.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// Holds the stop indicator.
        /// </summary>
        private bool stop = true;

        protected ForecastWorker(string id) : this(id, TenSeccondsInMilliseconds)
        {
        }

        protected ForecastWorker(string id, int sleepTimeInMilliseconds)
        {
            this.Id = id;
            this.SleepTime = new TimeSpan(0, 0, 0, 0, sleepTimeInMilliseconds);
        }

        /// <summary>
        /// Gets or sets the sleep time.
        /// </summary>
        public TimeSpan SleepTime { get; private set; }

        public bool IsStopped { get; private set; }

        protected string Id { get; private set; }

        /// <summary>
        /// Starts the process.
        /// </summary>
        public void Start()
        {
            string logMessage = string.Format(CultureInfo.CurrentCulture, "Starting worker of type '{0}'.", this.GetType().FullName);
            Logger.Info(logMessage);
            this.stop = false;

            // Multiple thread instances cannot be created
            if (this.thread == null || this.thread.ThreadState == ThreadState.Stopped)
            {
                this.thread = new Thread(this.Run);
            }

            // Start thread if it's not running yet
            if (this.thread.ThreadState != ThreadState.Running)
            {
                this.thread.Start();
            }
        }

        /// <summary>
        /// Stops the process.
        /// </summary>
        public void Stop()
        {
            string logMessage = string.Format(CultureInfo.CurrentCulture, "Stopping worker of type '{0}'.", this.GetType().FullName);
            Logger.Info(logMessage);
            this.stop = true;
        }

        public abstract void DoWork();

        protected static void WaitForResultOfRequest(ILog logger, string workerTypeName, string id, IOperation operation, Guid subscriptionId, string certificateThumbprint, string requestId)
        {
            OperationResult operationResult = new OperationResult();
            operationResult.Status = OperationStatus.InProgress;
            bool done = false;
            while (!done)
            {
                operationResult = operation.StatusCheck(subscriptionId, certificateThumbprint, requestId);
                if (operationResult.Status == OperationStatus.InProgress)
                {
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' submitted a deployment request with ID '{2}', the operation was found to be in process, waiting for '{3}' seconds.", workerTypeName, id, requestId, FiveSecondsInMilliseconds / 1000);
                    logger.Debug(logMessage);
                    Thread.Sleep(FiveSecondsInMilliseconds);
                }
                else
                {
                    done = true;
                }
            }

            if (operationResult.Status == OperationStatus.Failed)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' submitted a deployment request with ID '{2}' and it failed. The code was '{3}' and message '{4}'.", workerTypeName, id, requestId, operationResult.Code, operationResult.Message);
                logger.Error(logMessage);
            }
            else if (operationResult.Status == OperationStatus.Succeeded)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' submitted a deployment request with ID '{2}' and it succeeded. The code was '{3}' and message '{4}'.", workerTypeName, id, requestId, operationResult.Code, operationResult.Message);
                logger.Info(logMessage);
            }
        }

        protected static bool DetermineIfNowIsInTheSchedule(ILog logger, string workerTypeName, string id, TimeSpan dailyStartTime, TimeSpan dailyEndTime)
        {
            DateTime startTimeOfScheduleToday = DateTime.Today.Add(dailyStartTime);
            DateTime endTimeOfScheduleToday = DateTime.Today.Add(dailyEndTime);
            DateTime now = DateTime.Now;
            bool result;
            if (startTimeOfScheduleToday < now && now < endTimeOfScheduleToday)
            {
                string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' has found that now ('{2}') falls into the schedule with start time of '{3}' and end time of '{4}'.", workerTypeName, id, now, dailyStartTime, dailyEndTime);
                logger.Debug(deleteDeploymentLogMessage);
                result = true;
            }
            else
            {
                string deleteDeploymentLogMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' has found that now ('{2}') does not fall into the schedule with start time of '{3}' and end time of '{4}'.", workerTypeName, id, now, dailyStartTime, dailyEndTime);
                logger.Debug(deleteDeploymentLogMessage);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Run the task.
        /// </summary>
        private void Run()
        {
            try
            {
                try
                {
                    while (!this.stop)
                    {
                        this.IsStopped = false;
                        this.DoWork();
                        Thread.Sleep(this.SleepTime);
                    }
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                finally
                {
                    this.thread = null;
                    this.IsStopped = true;
                    string logMessage = string.Format(CultureInfo.CurrentCulture, "Stopped worker of type '{0}'.", this.GetType().FullName);
                    Logger.Info(logMessage);
                }
            }
            catch (Exception e)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "Error running the '{0}' worker.", this.GetType().FullName);
                Logger.Error(exceptionMessage, e);
                throw;
            }
        }
    }
}
