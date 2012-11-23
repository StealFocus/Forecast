namespace StealFocus.Forecast
{
    using System.Collections.Generic;

    using StealFocus.Forecast.Configuration;
    using StealFocus.Forecast.WindowsAzure;

    internal class Host
    {
        public Host()
        {
            this.ForecastWorkers = new List<ForecastWorker>();
            DeploymentDeleteForecastWorker[] deploymentDeleteForecastWorkers = StealFocusForecastConfiguration.Instance.GetDeploymentDeleteForecastWorkers();
            foreach (DeploymentDeleteForecastWorker deploymentDeleteForecastWorker in deploymentDeleteForecastWorkers)
            {
                this.ForecastWorkers.Add(deploymentDeleteForecastWorker);
            }
        }

        public List<ForecastWorker> ForecastWorkers { get; private set; }

        public void Start()
        {
            foreach (ForecastWorker forecastWorker in this.ForecastWorkers)
            {
                forecastWorker.Start();
            }
        }

        public void Stop()
        {
            foreach (ForecastWorker forecastWorker in this.ForecastWorkers)
            {
                forecastWorker.Stop();
            }
        }
    }
}
