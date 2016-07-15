using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public void AddToFavourites(int cityId, string cityName)
        {
            using (var context = new WeatherContext())
            {
                if (context.Cities.Count() >= 6)
                {
                    return;
                }
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

        public void RemoveFromFavourites(int cityId, string cityName)
        {
            using (var context = new WeatherContext())
            {
                var cityToRemove = context.Cities.First(city => city.ExternalIdentifier == cityId);
                context.Cities.Remove(cityToRemove);
                context.SaveChanges();
            }
        }

        public List<SearchHistoryRecord> GetSearchHistoryRecords()
        {
            using (var context = new WeatherContext())
            {
                return context.SearchHistoryRecords.ToList();
            }
        }

        public void AddToHistory(string cityName)
        {
            using (var context = new WeatherContext())
            {
                context.SearchHistoryRecords.Add(new SearchHistoryRecord { CityName = cityName, DateTime = DateTime.Now });
                context.SaveChanges();
            }
        }
    }
}
