using System;
using System.Reflection;
using Ninject;

namespace BSWeather.Services.Logger
{
    public class ThreadSafeLogger : ILogger
    {
        private readonly object _mutex = new object();
        private ILogPrinter _logPrinter;

        [Inject]
        public ILogPrinter LogPrinter
        {
            get { return _logPrinter; }
            set
            {
                if (value == null)
                {
                    Error(MethodBase.GetCurrentMethod().DeclaringType?.Name, "LogPrinter cannot be null!");
                    return;
                }
                lock (_mutex)
                {
                    _logPrinter = value;
                }
            }
        }

        public void Info(string message)
        {
            Info(null, message);
        }

        public void Info(string tag, string message)
        {
            lock (_mutex)
            {
                _logPrinter.Print(GetTaggedMessage(tag, message), MessageType.Info);
            }
        }

        public void Debug(string message)
        {
            Debug(null, message);
        }

        public void Debug(string tag, string message)
        {
            lock (_mutex)
            {
                _logPrinter.Print(GetTaggedMessage(tag, message), MessageType.Debug);
            }
        }

        public void Warning(Exception exception)
        {
            Warning(null, exception.Message);
        }

        public void Warning(string message)
        {
            Warning(null, message);
        }

        public void Warning(string tag, string message)
        {
            lock (_mutex)
            {
                _logPrinter.Print(GetTaggedMessage(tag, message), MessageType.Warning);
            }
        }

        public void Error(Exception exception)
        {
            Error(null, exception.Message);
        }

        public void Error(string message)
        {
            Error(null, message);
        }

        public void Error(string tag, string message)
        {
            lock (_mutex)
            {
                _logPrinter.Print(GetTaggedMessage(tag, message), MessageType.Error);
            }
        }

        public void Clear()
        {
            lock (_mutex)
            {
                _logPrinter.Clear();
            }
        }

        private static string GetTaggedMessage(string tag, string message)
        {
            var finalMessage = !string.IsNullOrEmpty(message) ? message : "Empty message";
            return string.IsNullOrEmpty(tag) ? finalMessage : tag + ": " + finalMessage;
        }
    }
}