namespace StealFocus.Forecast
{
    using System.Collections.Generic;
    using System.Reflection;

    using log4net;

    using StealFocus.Forecast.Configuration;
    using StealFocus.Forecast.WindowsAzure;

    internal class Host
    {
        private const int OneSecondInMilliseconds = 1000;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Host()
        {
            this.ForecastWorkers = new List<ForecastWorker>();
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

                System.Threading.Thread.Sleep(OneSecondInMilliseconds);
            }

            const string StoppedMessage = "...the workers were stopped.";
            Logger.Info(StoppedMessage);
        }
    }
}
