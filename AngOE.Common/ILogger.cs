using System;
using System.Collections.Generic;
using System.Linq;

namespace AngOE.Common
{
    public interface ILogger
    {
        bool IsDebugEnabled { get; }

        void Debug(object message, Exception e = null);
        void Info(object message, Exception e = null);
        void Error(object message, Exception e = null);

        void DebugFormat(string message, params object[] args);
        void InfoFormat(string message, params object[] args);
        void ErrorFormat(string message, params object[] args);
        List<string> GetRecentExceptionMessages();
    }


    public class Logger : ILogger
    {
        private const int MAX_LOGITEM_IN_MEMORY = 15;
        private static List<string> memoryExceptionLog = new List<string>();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsDebugEnabled
        {
            get
            {
                if (!isDebugEnabled.HasValue)
                {
                    isDebugEnabled = log.IsDebugEnabled;
                }

                return isDebugEnabled.Value;
            }
        }
        private bool? isDebugEnabled = null;

        public void Debug(object message, Exception e = null)
        {
            if (e == null)
            {
                log.Debug(message);
            }
            else
            {
                log.Debug(message, e);
            }
        }

        public void Info(object message, Exception e = null)
        {
            if (e == null)
            {
                log.Info(message);
            }
            else
            {
                log.Info(message, e);
            }
        }

        public void Error(object message, Exception e = null)
        {
            if (e == null)
            {
                log.Error(message);
            }
            else
            {
                log.Error(message, e);
            }

            if (IsDebugEnabled &&
                (e != null || (message is Exception)))
            {
                WriteToMemoryLog(e ?? (message as Exception));
            }
        }

        private void WriteToMemoryLog(Exception exception)
        {
            lock (memoryExceptionLog)
            {
                if (memoryExceptionLog.Count >= MAX_LOGITEM_IN_MEMORY)
                {
                    memoryExceptionLog = memoryExceptionLog.Skip(3).ToList();
                }

                var message = string.Format("{0}\n{1}", DateTime.UtcNow, exception.ToString());
                memoryExceptionLog.Add(message);
            }
        }

        public List<string> GetRecentExceptionMessages()
        {
            lock (memoryExceptionLog)
            {
                var result = new List<string>();
                result.Add("********* Recent Exceptions **********");

                var logEntries = memoryExceptionLog.ToList();
                logEntries.Reverse();

                result.AddRange(logEntries);

                return result;
            }
        }


        public void DebugFormat(string message, params object[] args)
        {
            log.DebugFormat(message, args);
        }

        public void InfoFormat(string message, params object[] args)
        {
            log.InfoFormat(message, args);
        }

        public void ErrorFormat(string message, params object[] args)
        {
            log.ErrorFormat(message, args);
        }
    }
}