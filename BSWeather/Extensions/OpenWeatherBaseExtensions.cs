using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSWeather.Models;
using BSWeather.Util;

namespace BSWeather.Extensions
{
    public static class OpenWeatherBaseExtensions
    {
        public static DateTime GetDateTime(this OpenWeatherBase.List weatherList)
        {
            return DateTimeUtils.UnixTimeStampToDateTime(weatherList.Dt);
        }
    }
}
