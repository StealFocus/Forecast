namespace StealFocus.Forecast.Console
{
    using System.Globalization;
    using System.Reflection;
    using Configuration;
    using log4net;
    using WindowsAzure;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            OutputVersionAndCopyrightMessage();
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

            System.Console.WriteLine("Press return to stop the workers.");
            System.Console.WriteLine();
            System.Console.ReadLine();
            foreach (DeploymentDeleteForecastWorker deploymentDeleteForecastWorker in deploymentDeleteForecastWorkers)
            {
                deploymentDeleteForecastWorker.Stop();
            }
        }

        private static void OutputVersionAndCopyrightMessage()
        {
            string line1 = string.Format(CultureInfo.CurrentCulture, "StealFocus Forecast Console Utility. Version {0}", typeof(Program).Assembly.GetName().Version);
            const string Line2 = "Copyright (c) StealFocus. All rights reserved.";
            System.Console.WriteLine(line1);
            System.Console.WriteLine(Line2);
            System.Console.WriteLine();
        }
    }
}
