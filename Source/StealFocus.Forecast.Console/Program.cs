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
            OutputVersionAndCopyrightMessage();
            if (args != null && args.Length > 0)
            {
                System.Console.WriteLine("The supplied arguments will be ignored.");
                System.Console.WriteLine();
            }

            Host host = new Host();
            host.Start();
            System.Console.WriteLine("Press return to stop the workers.");
            System.Console.ReadLine();
            host.Stop(Logger);
            host.WaitForWorkersToStop(Logger);
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
