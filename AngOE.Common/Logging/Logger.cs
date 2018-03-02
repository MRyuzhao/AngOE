using System;
using log4net;

namespace AngOE.Common.Logging
{
    public class Logger
    {
        private static readonly ILog Log = LogManager.GetLogger("Logger");

        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void Info(string format, params object[] args)
        {
            Log.Info(string.Format(format, args));
        }

        public static void Info(string message, Exception exception)
        {
            Log.Info(message, exception);
        }

        public static void Warn(string format, params object[] args)
        {
            Log.Warn(string.Format(format, args));
        }

        public static void Warn(string message, Exception exception)
        {
            Log.Warn(message, exception);
        }

        public static void Error(string format, params object[] args)
        {
            Log.Error(string.Format(format, args));
        }

        public static void Error(string message, Exception exception)
        {
            Log.Error(message, exception);
        }
    }
}