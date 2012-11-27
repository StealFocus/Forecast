namespace StealFocus.Forecast.Console
{
    using System.Globalization;
    using System.Reflection;

    using log4net;

    internal class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal static void Main(string[] args)
        {
            string title = string.Format(CultureInfo.CurrentCulture, "StealFocus Forecast Console Utility. Version {0}", typeof(Program).Assembly.GetName().Version);
            Host.OutputVersionAndCopyrightMessage(title);
            if (args != null && args.Length > 0)
            {
                const string WarningMessage = "The supplied arguments will be ignored.";
                Logger.Info(WarningMessage);
                Logger.Info(string.Empty);
            }

            Host host = new Host();
            host.Start();
            System.Console.WriteLine("Press return to stop the workers.");
            System.Console.ReadLine();
            host.Stop();
            host.WaitForWorkersToStop();
        }
    }
}
