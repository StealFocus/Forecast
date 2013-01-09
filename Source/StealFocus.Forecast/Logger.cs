namespace StealFocus.Forecast
{
    using System.Globalization;
    using System.IO;

    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;

    internal static class Logger
    {
        private const string ApplicationName = "StealFocus Forecast";

        private const string CurrentDirectory = ".";

        private const string EventLogName = "Application";

        private const string LogPattern = "%date [%thread] %-5level %logger %property - %message%newline";

        public static void Configure()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = LogPattern;
            patternLayout.ActivateOptions();
            RollingFileAppender rollingFileAppender = GetRollingFileAppender(patternLayout);
            hierarchy.Root.AddAppender(rollingFileAppender);
            EventLogAppender eventLogAppender = GetEventLogAppender(patternLayout);
            hierarchy.Root.AddAppender(eventLogAppender);
            OutputDebugStringAppender outputDebugStringAppender = GetOutputDebugStringAppender(patternLayout);
            hierarchy.Root.AddAppender(outputDebugStringAppender);
            ColoredConsoleAppender coloredConsoleAppender = GetColoredConsoleAppender(patternLayout);
            hierarchy.Root.AddAppender(coloredConsoleAppender);
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        private static RollingFileAppender GetRollingFileAppender(PatternLayout patternLayout)
        {
            RollingFileAppender rollingFileAppender = new RollingFileAppender();
            rollingFileAppender.File = GetRollingFileAppenderLogFilePath();
            rollingFileAppender.AppendToFile = true;
            rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Size;
            rollingFileAppender.MaxSizeRollBackups = 100;
            rollingFileAppender.MaximumFileSize = "10240KB";
            rollingFileAppender.StaticLogFileName = true;
            rollingFileAppender.Layout = patternLayout;
            rollingFileAppender.Threshold = Level.All;
            rollingFileAppender.ActivateOptions();
            return rollingFileAppender;
        }

        private static string GetRollingFileAppenderLogFilePath()
        {
            string logFileName = string.Format(CultureInfo.CurrentCulture, "{0}.log", ApplicationName);
            return Path.Combine(CurrentDirectory, logFileName);
        }

        private static EventLogAppender GetEventLogAppender(PatternLayout patternLayout)
        {
            EventLogAppender eventLogAppender = new EventLogAppender();
            eventLogAppender.LogName = EventLogName;
            eventLogAppender.ApplicationName = ApplicationName;
            eventLogAppender.Layout = patternLayout;
            eventLogAppender.Threshold = Level.Warn;
            eventLogAppender.ActivateOptions();
            return eventLogAppender;
        }

        private static OutputDebugStringAppender GetOutputDebugStringAppender(PatternLayout patternLayout)
        {
            OutputDebugStringAppender outputDebugStringAppender = new OutputDebugStringAppender();
            outputDebugStringAppender.Layout = patternLayout;
            outputDebugStringAppender.Threshold = Level.All;
            outputDebugStringAppender.ActivateOptions();
            return outputDebugStringAppender;
        }

        private static ColoredConsoleAppender GetColoredConsoleAppender(PatternLayout patternLayout)
        {
            ColoredConsoleAppender coloredConsoleAppender = new ColoredConsoleAppender();
            coloredConsoleAppender.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Fatal,
                    ForeColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity
                });
            coloredConsoleAppender.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Error,
                    ForeColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity
                });
            coloredConsoleAppender.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Warn,
                    ForeColor = ColoredConsoleAppender.Colors.Yellow | ColoredConsoleAppender.Colors.HighIntensity
                });
            coloredConsoleAppender.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Info,
                    ForeColor = ColoredConsoleAppender.Colors.White
                });
            coloredConsoleAppender.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Debug,
                    ForeColor = ColoredConsoleAppender.Colors.Green
                });
            coloredConsoleAppender.Layout = patternLayout;
            coloredConsoleAppender.Threshold = Level.All;
            coloredConsoleAppender.ActivateOptions();
            return coloredConsoleAppender;
        }
    }
}
