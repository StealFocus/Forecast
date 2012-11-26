﻿namespace StealFocus.Forecast.Windows.Service
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using log4net;

    using Topshelf;

    internal class Program
    {
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
                            string title = string.Format(CultureInfo.CurrentCulture, "StealFocus Forecast Windows Service. Version {0}", typeof(Program).Assembly.GetName().Version);
                            Forecast.Host.OutputVersionAndCopyrightMessage(title);
                            host.Start();
                        });
                    serviceConfigurator.WhenStopped(host =>
                        {
                            host.Stop();
                            host.WaitForWorkersToStop();
                        });
                });
                hostConfigurator.RunAsPrompt(); // Prompt for the service credentials when installing the service.
                hostConfigurator.SetDescription("The StealFocus Forecast Windows Service.");
                Version version = typeof(Program).Assembly.GetName().Version;
                string displayName = string.Format(CultureInfo.CurrentCulture, "StealFocus Forecast Windows Service v{0}", version);
                hostConfigurator.SetDisplayName(displayName);
                hostConfigurator.SetServiceName(displayName.Replace(' ', '.')); // No spaces allowed in service name
                hostConfigurator.StartAutomaticallyDelayed();
            });
        }
    }
}
