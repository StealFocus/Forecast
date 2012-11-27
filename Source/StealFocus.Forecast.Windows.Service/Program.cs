namespace StealFocus.Forecast.Windows.Service
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
            CheckArguments(args);
            ConfigureWindowsService();
        }

        private static void CheckArguments(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                bool installArgumentFound = false;
                bool usernameArgumentFound = false;
                foreach (string arg in args)
                {
                    if (arg == "install")
                    {
                        installArgumentFound = true;
                    }

                    if (arg.StartsWith("-username:", StringComparison.CurrentCultureIgnoreCase))
                    {
                        usernameArgumentFound = true;
                    }
                }

                if (installArgumentFound)
                {
                    if (!usernameArgumentFound)
                    {
                        Logger.Warn("Without supplying the username and password arguments, this service will install as 'Local System'.");
                    }

                    Logger.Warn("The account under which this service will run must have certificates matching the thumbprints of those in the configuration.");
                }
            }
        }

        private static void ConfigureWindowsService()
        {
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
                Version version = typeof(Program).Assembly.GetName().Version;
                string displayName = string.Format(CultureInfo.CurrentCulture, "StealFocus Forecast Windows Service v{0}", version);
                hostConfigurator.SetDisplayName(displayName);
                hostConfigurator.SetServiceName(displayName.Replace(' ', '.')); // No spaces allowed in service name
                hostConfigurator.StartAutomaticallyDelayed();

                // Enable automatic service recovery.
                hostConfigurator.EnableServiceRecovery(src =>
                {
                    src.RestartService(10); // Wait 10 minutes before restarting.
                    src.SetResetPeriod(1); // Set the reset interval to one day.
                });
            });
        }
    }
}
