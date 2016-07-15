using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BSWeather.Infrastructure.Context;
using BSWeather.Models;

namespace BSWeather.Services
{
    public class BsWeatherService
    {
        public void FillViewData(ViewDataDictionary viewData, OpenWeatherBase.RootObject weather, int days)
        {
            List<City> favouriteCities;
            using (var context = new WeatherContext())
            {
                favouriteCities = context.Cities.ToList();
            }

            viewData["Weather"] = weather;
            viewData["Days"] = days;
            viewData["FavouriteCities"] = favouriteCities;
        }

        public void AddToHistrory(int cityId, string cityName)
        {
            using (var context = new WeatherContext())
            {
                bool contains = context.Cities.Any(city => city.ExternalIdentifier == cityId);
                if (!contains)
                {
                    context.Cities.Add(new City
                    {
                        ExternalIdentifier = cityId,
                        Name = cityName
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
