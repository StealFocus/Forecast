namespace StealFocus.Forecast
{
    using System;
    using System.Threading;

    public abstract class ForecastWorker
    {
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
            this.SleepPeriod = new TimeSpan(0, 10, 0);
        }

        /// <summary>
        /// Gets or sets the sleep period.
        /// </summary>
        /// <remarks>
        /// Defaults to 10 minutes.
        /// </remarks>
        public TimeSpan SleepPeriod { get; set; }

        /// <summary>
        /// Starts the process.
        /// </summary>
        public void Start()
        {
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
            catch (Exception)
            {
                // this.Logger.Error("Error running the Work Item Summary Sender.", e);
                throw;
            }
        }
    }
}
