namespace StealFocus.Forecast
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
            if (args != null && args.Length > 0)
            {
                Logger.Info(string.Empty);
                Logger.Info("Command line arguments supplied so using those arguments to install/uninstall as a windows service.");
                Logger.Info(string.Empty);
                Logger.Info("To run as a console application, run with no arguments.");
                Logger.Info(string.Empty);
                InstallAsWindowsServiceUsingTopShelf(args);
            }
            else
            {
                Logger.Info(string.Empty);
                Logger.Info("No command line arguments supplied so running in console mode.");
                Logger.Info(string.Empty);
                Logger.Info("If you wish to install/uninstall as a windows please supply 'install' or");
                Logger.Info("'uninstall' arguments as appropriate.");
                RunAsConsole();
            }
        }

        private static void OutputVersionAndCopyrightMessage(string title)
        {
            string line1 = title;
            const string Line2 = "Copyright (c) StealFocus. All rights reserved.";
            Logger.Info(string.Empty);
            Logger.Info(line1);
            Logger.Info(Line2);
            Logger.Info(string.Empty);
        }

        private static void RunAsConsole()
        {
            string title = string.Format(CultureInfo.CurrentCulture, "StealFocus Forecast Console. Version {0}", typeof(Program).Assembly.GetName().Version);
            OutputVersionAndCopyrightMessage(title);
            Host host = new Host();
            host.Start();

            // Write with the console (and not via the logger) to guarantee this message
            // is displayed to the user (in case the logger is misconfigured).
            System.Console.WriteLine("Press return to stop the workers.");
            System.Console.ReadLine();
            host.Stop();
            host.WaitForWorkersToStop();
        }

        private static void InstallAsWindowsServiceUsingTopShelf(string[] args)
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
                        Logger.Warn("Without supplying the username ('-username:???') and password ('-password:???')");
                        Logger.Warn("arguments, this service will install as 'Local System'.");
                        Logger.Warn(string.Empty);
                    }

                    Logger.Warn("The account under which this service will run must have certificates, matching");
                    Logger.Warn("the thumbprints of those in the configuration, available in the 'Personal'");
                    Logger.Warn("certificate store (run 'certmgr.msc' for certificate admin features).");
                    Logger.Warn(string.Empty);
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
                        OutputVersionAndCopyrightMessage(title);
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
