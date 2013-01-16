namespace StealFocus.Forecast
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;

    using log4net;

    using StealFocus.Forecast.Configuration;
    using StealFocus.Forecast.WindowsAzure.HostedService;
    using StealFocus.Forecast.WindowsAzure.StorageService;

    internal class Host
    {
        private const int OneSecondInMilliseconds = 1000;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Host()
        {
            this.ForecastWorkers = new List<ForecastWorker>();
            this.ForecastWorkers.Add(StealFocusForecastConfiguration.GetWhiteListForecastWorker());
            DeploymentDeleteForecastWorker[] deploymentDeleteForecastWorkers = StealFocusForecastConfiguration.GetDeploymentDeleteForecastWorkers();
            foreach (DeploymentDeleteForecastWorker deploymentDeleteForecastWorker in deploymentDeleteForecastWorkers)
            {
                this.ForecastWorkers.Add(deploymentDeleteForecastWorker);
            }

            DeploymentCreateForecastWorker[] deploymentCreateForecastWorkers = StealFocusForecastConfiguration.GetDeploymentCreateForecastWorkers();
            foreach (DeploymentCreateForecastWorker deploymentCreateForecastWorker in deploymentCreateForecastWorkers)
            {
                this.ForecastWorkers.Add(deploymentCreateForecastWorker);
            }

            ScheduledHorizontalScaleForecastWorker[] scheduledHorizontalScaleForecastWorkers = StealFocusForecastConfiguration.GetScheduledHorizontalScaleForecastWorkers();
            foreach (ScheduledHorizontalScaleForecastWorker scheduledHorizontalScaleForecastWorker in scheduledHorizontalScaleForecastWorkers)
            {
                this.ForecastWorkers.Add(scheduledHorizontalScaleForecastWorker);
            }

            TableDeleteForecastWorker[] tableDeleteForecastWorkers = StealFocusForecastConfiguration.GetTableDeleteForecastWorkers();
            foreach (TableDeleteForecastWorker tableDeleteForecastWorker in tableDeleteForecastWorkers)
            {
                this.ForecastWorkers.Add(tableDeleteForecastWorker);
            }

            BlobContainerDeleteForecastWorker[] blobContainerDeleteForecastWorkers = StealFocusForecastConfiguration.GetBlobContainerDeleteForecastWorkers();
            foreach (BlobContainerDeleteForecastWorker blobContainerDeleteForecastWorker in blobContainerDeleteForecastWorkers)
            {
                this.ForecastWorkers.Add(blobContainerDeleteForecastWorker);
            }
        }

        private List<ForecastWorker> ForecastWorkers { get; set; }

        public void Start()
        {
            foreach (ForecastWorker forecastWorker in this.ForecastWorkers)
            {
                forecastWorker.Start();
            }
        }

        public void Stop()
        {
            const string StoppingMessage = "Stopping the workers...";
            Logger.Info(StoppingMessage);
            foreach (ForecastWorker forecastWorker in this.ForecastWorkers)
            {
                forecastWorker.Stop();
            }
        }

        public void WaitForWorkersToStop()
        {
            bool keepPolling = true;
            while (keepPolling)
            {
                const string CheckingMessage = "Checking, please wait.";
                Logger.Info(CheckingMessage);
                keepPolling = false;
                foreach (ForecastWorker forecastWorker in this.ForecastWorkers)
                {
                    if (forecastWorker.IsStopped == false)
                    {
                        keepPolling = true;
                    }
                }

                Thread.Sleep(OneSecondInMilliseconds);
            }

            const string StoppedMessage = "...the workers were stopped.";
            Logger.Info(StoppedMessage);
        }
    }
}
