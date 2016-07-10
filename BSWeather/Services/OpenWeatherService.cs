using System;
using System.Net;
using System.Text;
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
        private static readonly string BaseUrl = "http://openweathermap.org";
        private static readonly string ApiUrl = "http://api.openweathermap.org";

        [Inject]
        private ILogger Logger { get; set; }

        [Inject]
        private AvailabilityCheckService AvailabilityCheckService { get; set; }

        public OpenWeatherBase.RootObject GetWeatherById(int cityId, int days)
        {
            var url = $"{ApiUrl}/data/2.5/forecast/daily?id={cityId}&units=metric&lang=ru&cnt={days}&APPID={WebConfigurationManager.AppSettings["OpenWeatherMapAPIKEY"]}";
            return GetWeatherByUrl(url);
        }

        public OpenWeatherBase.RootObject GetWeatherByCityName(string cityName, int days)
        {
            var url = $"{ApiUrl}/data/2.5/forecast/daily?q={cityName}&units=metric&lang=ru&cnt={days}&APPID={WebConfigurationManager.AppSettings["OpenWeatherMapAPIKEY"]}";
            return GetWeatherByUrl(url);
        }

        public OpenWeatherBase.RootObject GetWeatherByUrl(string url)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            string resultString;

            try
            {
                resultString = webClient.DownloadString(url);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                AvailabilityCheckService.CheckAvailable(BaseUrl);
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<OpenWeatherBase.RootObject>(resultString);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }
    }
}
