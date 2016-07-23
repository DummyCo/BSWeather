using System;
using System.Net.Http;
using System.Threading.Tasks;
using BSWeather.Services.Logger;
using Ninject;

namespace BSWeather.Services
{
    public class AvailabilityCheckService
    {
        [Inject]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private ILogger Logger { get; set; }

        public async Task<bool> CheckAvailable(string url)
        {
            try
            {
                using (var client = new HttpClient {Timeout = new TimeSpan(0, 0, 3)})
                using (var request = new HttpRequestMessage(HttpMethod.Head, url))
                using (var response = await client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Logger.Info($"{url} is not avaliable");
                        return false;
                    }
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