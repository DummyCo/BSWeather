using System;
using System.Net;
using System.Text;
using System.Web.Configuration;
using BSWeather.Models;
using BSWeather.Services.Logger;
using Newtonsoft.Json;
using Ninject;

namespace BSWeather.Services
{
    public class OpenWeatherService
    {
        private static readonly string BaseUrl = "http://api.openweathermap.org";

        [Inject]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private ILogger Logger { get; set; }

        public OpenWeatherBase.RootObject GetWeatherById(int cityId, int days)
        {
            var url = $"{BaseUrl}/data/2.5/forecast/daily?id={cityId}&units=metric&lang=ru&cnt={days}&APPID={WebConfigurationManager.AppSettings["OpenWeatherMapAPIKEY"]}";
            return GetWeatherByUrl(url);
        }

        public OpenWeatherBase.RootObject GetWeatherByCityName(string cityName, int days)
        {
            var url = $"{BaseUrl}/data/2.5/forecast/daily?q={cityName}&units=metric&lang=ru&cnt={days}&APPID={WebConfigurationManager.AppSettings["OpenWeatherMapAPIKEY"]}";
            return GetWeatherByUrl(url);
        }

        public OpenWeatherBase.RootObject GetWeatherByUrl(string url)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            var resultString = webClient.DownloadString(url);

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
