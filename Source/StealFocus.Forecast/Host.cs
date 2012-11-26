namespace StealFocus.Forecast
{
    using System;
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

        public static void OutputVersionAndCopyrightMessage(string title)
        {
            string line1 = title;
            const string Line2 = "Copyright (c) StealFocus. All rights reserved.";
            Console.WriteLine();
            Console.WriteLine(line1);
            Console.WriteLine(Line2);
            Console.WriteLine();
            Logger.Info(string.Empty);
            Logger.Info(line1);
            Logger.Info(Line2);
            Logger.Info(string.Empty);
        }

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
            Console.WriteLine(StoppingMessage);
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
                Console.WriteLine(CheckingMessage);
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
            Console.WriteLine(StoppedMessage);
            Logger.Info(StoppedMessage);
        }
    }
}
