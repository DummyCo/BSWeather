using System;
using System.IO;

namespace BSWeather.Services.Logger
{
    public class StreamWriterLogPrinter : ILogPrinter
    {
        private readonly string _logFilePath;

        public StreamWriterLogPrinter(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Print(string message, MessageType messageType)
        {
            using (var writer = new StreamWriter(_logFilePath, true))
            {
                writer.WriteLine(DateTime.Now + " " + messageType + "/" + message);
            }
        }

        public void Clear()
        {
            using (var writer = new StreamWriter(_logFilePath, false))
            {
                writer.Write(string.Empty);
            }
        }
    }
}