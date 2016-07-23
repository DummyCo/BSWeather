using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using BSWeather.Models;
using BSWeather.Services.Logger;
using Newtonsoft.Json;
using Ninject;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BSWeather.Services
{
    public class OpenWeatherService
    {
        private const string BaseUrl = "http://openweathermap.org";
        private const string ApiUrl = "http://api.openweathermap.org";

        [Inject]
        private ILogger Logger { get; set; }

        [Inject]
        private AvailabilityCheckService AvailabilityCheckService { get; set; }

        public async Task<OpenWeatherBase.RootObject> GetWeatherByIdAsync(int cityId, int days)
        {
            var url = $"{ApiUrl}/data/2.5/forecast/daily?id={cityId}&units=metric&lang=ru&cnt={days}&APPID={WebConfigurationManager.AppSettings["OpenWeatherMapAPIKEY"]}";
            return await GetWeatherByUrlAsync(url);
        }

        public async Task<OpenWeatherBase.RootObject> GetWeatherByCityNameAsync(string cityName, int days)
        {
            var url = $"{ApiUrl}/data/2.5/forecast/daily?q={cityName}&units=metric&lang=ru&cnt={days}&APPID={WebConfigurationManager.AppSettings["OpenWeatherMapAPIKEY"]}";
            return await GetWeatherByUrlAsync(url);
        }

        public async Task<OpenWeatherBase.RootObject> GetWeatherByUrlAsync(string url)
        {
            string resultString;

            try
            {
                using (var client = new HttpClient())
                using (var response = await client.GetAsync(url))
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    resultString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                await AvailabilityCheckService.CheckAvailable(BaseUrl);
                return null;
            }

            try
            {
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<OpenWeatherBase.RootObject>(resultString));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }
    }
}
