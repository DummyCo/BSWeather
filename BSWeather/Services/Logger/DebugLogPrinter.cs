using System.Diagnostics;

namespace BSWeather.Services.Logger
{
    public class DebugLogPrinter : ILogPrinter
    {
        public void Print(string message, MessageType messageType)
        {
            Debug.Write(message);
        }

        public void Clear()
        {
            
        }
    }
}
