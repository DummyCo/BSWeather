using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using BSWeather.Models;
using Newtonsoft.Json;

namespace BSWeather.Services
{
    public class OpenWeatherService
    {
        public OpenWeatherBase.RootObject GetWeatherById(int cityId)
        {
            var url = $"http://api.openweathermap.org/data/2.5/forecast/daily?id={cityId}&units=metric&APPID={WebConfigurationManager.AppSettings["OpenWeatherMapAPIKEY"]}";
            var webClient = new WebClient();
            var resultString = webClient.DownloadString(url);

            return JsonConvert.DeserializeObject<OpenWeatherBase.RootObject>(resultString);
        }

        public OpenWeatherBase.RootObject GetWeatherByCityName(string cityName)
        {
            var url = $"http://api.openweathermap.org/data/2.5/forecast/daily?q={cityName}&units=metric&APPID={WebConfigurationManager.AppSettings["OpenWeatherMapAPIKEY"]}";
            var webClient = new WebClient();
            var resultString = webClient.DownloadString(url);

            return JsonConvert.DeserializeObject<OpenWeatherBase.RootObject>(resultString);
        }
    }
}
