using System.Net;
using BSWeather.Services.Logger;
using Ninject;

namespace BSWeather.Services
{
    public class AvailabilityCheckService
    {
        [Inject]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private ILogger Logger { get; set; }

        public bool CheckAvailable(string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 3000;
                request.AllowAutoRedirect = false;
                request.Method = "HEAD";

                using (request.GetResponse())
                {
                    Logger.Info($"{url} is avaliable");
                    return true;
                }
            }
            catch
            {
                Logger.Info($"{url} is not avaliable");
                return false;
            }
        }
    }
}
