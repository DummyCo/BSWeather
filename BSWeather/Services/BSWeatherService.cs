using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BSWeather.Infrastructure;
using BSWeather.Infrastructure.Context;
using BSWeather.Models;

namespace BSWeather.Services
{
    public class BsWeatherService
    {
        public List<City> DeafultFavouriteCities
        {
            get
            {
                using (var context = new WeatherContext())
                {
                    return context.Cities.ToList().GetRange(0, 5);
                }
            }
        }

        public City TrackCity(int cityId, string cityName)
        {
            using (var context = new WeatherContext())
            {
                var city = context.Cities.FirstOrDefault(c => c.ExternalIdentifier == cityId);
                if (city == null)
                {
                    var newCity = new City
                    {
                        ExternalIdentifier = cityId,
                        Name = cityName
                    };
                    context.Cities.Add(newCity);
                    context.SaveChanges();
                    return newCity;
                }
                return city;
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
