namespace StealFocus.Forecast.Console
{
    using System.Globalization;
    using System.Reflection;
    using Configuration;
    using log4net;
    using WindowsAzure;

    internal class Program
    {
        private const int OneSecondInMilliseconds = 1000;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            System.Console.ReadLine();
            System.Console.WriteLine("Stopping the workers...");
            foreach (DeploymentDeleteForecastWorker deploymentDeleteForecastWorker in deploymentDeleteForecastWorkers)
            {
                deploymentDeleteForecastWorker.Stop();
            }

            bool keepPolling = true;
            while (keepPolling)
            {
                keepPolling = false;
                foreach (DeploymentDeleteForecastWorker deploymentDeleteForecastWorker in deploymentDeleteForecastWorkers)
                {
                    System.Console.WriteLine("Checking, please wait.");
                    if (deploymentDeleteForecastWorker.IsStopped == false)
                    {
                        keepPolling = true;
                    }
                }

                System.Threading.Thread.Sleep(OneSecondInMilliseconds);
            }

            System.Console.WriteLine("...the workers were stopped.");
        }

        private static void OutputVersionAndCopyrightMessage()
        {
            string line1 = string.Format(CultureInfo.CurrentCulture, "StealFocus Forecast Console Utility. Version {0}", typeof(Program).Assembly.GetName().Version);
            const string Line2 = "Copyright (c) StealFocus. All rights reserved.";
            System.Console.WriteLine();
            System.Console.WriteLine(line1);
            System.Console.WriteLine(Line2);
            System.Console.WriteLine();
            Logger.Info(string.Empty);
            Logger.Info(line1);
            Logger.Info(Line2);
            Logger.Info(string.Empty);
        }
    }
}
