﻿namespace StealFocus.Forecast
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;
    using log4net;

    public abstract class ForecastWorker
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Holds the thread.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// Holds the stop indicator.
        /// </summary>
        private bool stop = true;

        protected ForecastWorker()
        {
            this.SleepPeriod = new TimeSpan(0, 0, 0, 10);
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the sleep period.
        /// </summary>
        /// <remarks>
        /// Defaults to 10 minutes.
        /// </remarks>
        public TimeSpan SleepPeriod { get; set; }

        protected Guid Id { get; private set; }

        /// <summary>
        /// Starts the process.
        /// </summary>
        public void Start()
        {
            string logMessage = string.Format(CultureInfo.CurrentCulture, "Starting worker of type '{0}'.", this.GetType().FullName);
            logger.Info(logMessage);
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
            logger.Info(logMessage);
            this.stop = true;
        }

        protected abstract void DoWork();

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
                        this.DoWork();
                        Thread.Sleep(this.SleepPeriod);
                    }
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                finally
                {
                    this.thread = null;
                }
            }
            catch (Exception e)
            {
                logger.Error("Error running the Work Item Summary Sender.", e);
                throw;
            }
        }
    }
}
