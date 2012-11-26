namespace StealFocus.Forecast
{
    using System;
    using System.Collections.Generic;

    using log4net;

    using StealFocus.Forecast.Configuration;
    using StealFocus.Forecast.WindowsAzure;

    internal class Host
    {
        private const int OneSecondInMilliseconds = 1000;

        public Host()
        {
            this.ForecastWorkers = new List<ForecastWorker>();
            DeploymentDeleteForecastWorker[] deploymentDeleteForecastWorkers = StealFocusForecastConfiguration.Instance.GetDeploymentDeleteForecastWorkers();
            foreach (DeploymentDeleteForecastWorker deploymentDeleteForecastWorker in deploymentDeleteForecastWorkers)
            {
                this.ForecastWorkers.Add(deploymentDeleteForecastWorker);
            }

            DeploymentCreateForecastWorker[] deploymentCreateForecastWorkers = StealFocusForecastConfiguration.Instance.GetDeploymentCreateForecastWorkers();
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

        public void Stop(ILog logger)
        {
            const string StoppingMessage = "Stopping the workers...";
            Console.WriteLine(StoppingMessage);
            logger.Info(StoppingMessage);
            foreach (ForecastWorker forecastWorker in this.ForecastWorkers)
            {
                forecastWorker.Stop();
            }
        }

        public void WaitForWorkersToStop(ILog logger)
        {
            bool keepPolling = true;
            while (keepPolling)
            {
                const string CheckingMessage = "Checking, please wait.";
                Console.WriteLine(CheckingMessage);
                logger.Info(CheckingMessage);
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
            Console.WriteLine(StoppedMessage);
            logger.Info(StoppedMessage);
        }
    }
}
