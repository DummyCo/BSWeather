namespace BSWeather.Services.Logger
{
    public interface ILogPrinter
    {
        void Print(string message, MessageType messageType);

        void Clear();
    }
}