namespace StealFocus.Forecast.Console
{
    using Configuration;
    using WindowsAzure;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                System.Console.WriteLine("Supplied arguments:");
                System.Console.WriteLine(arg);
                System.Console.WriteLine();
            }

            DeploymentDeleteForecastWorker[] deploymentDeleteForecastWorkers = StealFocusForecastConfiguration.Instance.GetDeploymentDeleteForecastWorkers();
            foreach (DeploymentDeleteForecastWorker deploymentDeleteForecastWorker in deploymentDeleteForecastWorkers)
            {
                deploymentDeleteForecastWorker.Start();
            }

            System.Console.ReadLine();
        }
    }
}
