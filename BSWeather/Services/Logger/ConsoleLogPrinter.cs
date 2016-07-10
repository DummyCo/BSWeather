using System;

namespace BSWeather.Services.Logger
{
    public class ConsoleLogPrinter : ILogPrinter
    {
        public void Print(string message, MessageType messageType)
        {
            switch (messageType)
            {
                default:
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                }
                case MessageType.Debug:
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                case MessageType.Warning:
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                }
                case MessageType.Error:
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                }
            }
            Console.WriteLine(DateTime.Now + " " + message);
            Console.ResetColor();
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}