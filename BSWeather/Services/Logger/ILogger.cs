using System;

namespace BSWeather.Services.Logger
{
    public interface ILogger
    {
        ILogPrinter LogPrinter { get; set; }
        void Info(string message);
        void Info(string tag, string message);
        void Debug(string message);
        void Debug(string tag, string message);
        void Warning(Exception exception);
        void Warning(string message);
        void Warning(string tag, string message);
        void Error(Exception exception);
        void Error(string message);
        void Error(string tag, string message);
        void Clear();
    }

    public enum MessageType
    {
        Info,
        Debug,
        Warning,
        Error
    }
}