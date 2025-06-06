using System;
using System.IO;
using NLog;

namespace Project3.Services
{
    public class FileLogger : ILogger
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static FileLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app-${shortdate}.log"),
                Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=tostring}"
            };
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogError(string message, Exception? ex = null)
        {
            if (ex != null)
                _logger.Error(ex, message);
            else
                _logger.Error(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warn(message);
        }
    }
}
