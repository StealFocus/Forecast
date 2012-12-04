namespace StealFocus.Forecast
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;

    using log4net;

    internal abstract class ForecastWorker
    {
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

        protected bool DetermineIfNowIsInTheSchedule(ILog logger, string workerTypeName, IEnumerable<ScheduleDay> scheduleDays)
        {
            DateTime now = DateTime.Now;
            foreach (ScheduleDay scheduleDay in scheduleDays)
            {
                if (scheduleDay.DayOfWeek == now.DayOfWeek)
                {
                    DateTime startTimeOfScheduleToday = DateTime.Today.Add(scheduleDay.StartTime);
                    DateTime endTimeOfScheduleToday = DateTime.Today.Add(scheduleDay.EndTime);
                    if (startTimeOfScheduleToday < now && now < endTimeOfScheduleToday)
                    {
                        string logNowInScheduleMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' has found that now ('{2}') falls into the schedule of '{3}' with start time of '{4}' and end time of '{5}'.", workerTypeName, this.Id, now, scheduleDay.DayOfWeek, scheduleDay.StartTime, scheduleDay.EndTime);
                        logger.Debug(logNowInScheduleMessage);
                        return true;
                    }
                }
            }

            string logNowNotInScheduleMessage = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' has found that now ('{2}') does not fall into any of the schedules.", workerTypeName, this.Id, now);
            logger.Debug(logNowNotInScheduleMessage);
            return false;
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
