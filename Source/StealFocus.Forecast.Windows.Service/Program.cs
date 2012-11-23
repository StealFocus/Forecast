namespace StealFocus.Forecast.Windows.Service
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using log4net;

    using Topshelf;

    internal class Program
    {
        private const int OneSecondInMilliseconds = 1000;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal static void Main(string[] args)
        {
            if (args != null && args.Length > 1)
            {
                const string WarningMessage = "Additional arguments will be ignored.";
                Logger.Info(WarningMessage);
                Console.WriteLine(WarningMessage);
                Console.WriteLine();
            }

            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.Service<Forecast.Host>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(name => new Forecast.Host());
                    serviceConfigurator.WhenStarted(host =>
                        {
                            OutputVersionAndCopyrightMessage();
                            host.Start();
                        });
                    serviceConfigurator.WhenStopped(host =>
                        {
                            const string StoppingMessage = "Stopping the workers...";
                            Console.WriteLine(StoppingMessage);
                            Logger.Info(StoppingMessage);
                            host.Stop();
                            bool keepPolling = true;
                            while (keepPolling)
                            {
                                keepPolling = false;
                                foreach (ForecastWorker forecastWorker in host.ForecastWorkers)
                                {
                                    const string CheckingMessage = "Checking, please wait.";
                                    Console.WriteLine(CheckingMessage);
                                    Logger.Info(CheckingMessage);
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
                        });
                });
                hostConfigurator.RunAsLocalSystem();
                hostConfigurator.SetDescription("The StealFocus Forecast Windows Service.");
                Version version = typeof(Program).Assembly.GetName().Version;
                string displayName = string.Format(CultureInfo.CurrentCulture, "StealFocus Forecast Windows Service v{0}", version);
                hostConfigurator.SetDisplayName(displayName);
                hostConfigurator.SetServiceName(displayName.Replace(' ', '.')); // No spaces allowed in service name
                hostConfigurator.StartAutomatically();
            });
        }

        private static void OutputVersionAndCopyrightMessage()
        {
            string line1 = string.Format(CultureInfo.CurrentCulture, "StealFocus Forecast Windows Service. Version {0}", typeof(Program).Assembly.GetName().Version);
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
    }
}
