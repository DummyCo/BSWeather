using System;
using System.IO;

namespace BSWeather.Services.Logger
{
    public class FileLogPrinter : ILogPrinter
    {
        private readonly string _logFilePath;

        public FileLogPrinter(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Print(string message, MessageType messageType)
        {
            File.AppendAllText(_logFilePath, DateTime.Now + " " + messageType + "/" + message + Environment.NewLine);
        }

        public void Clear()
        {
            File.WriteAllText(_logFilePath, string.Empty);
        }
    }
}