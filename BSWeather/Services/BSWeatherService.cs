using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddToFavourites(WeatherContext context, User user, City city)
        {
            context.Users.Attach(user);
            context.Cities.Attach(city);

            if (user.Cities.Count < 6)
            {
                user.Cities.Add(city);
                city.Users.Add(user);
            }

            context.SaveChanges();
        }

        public void RemoveFromFavourites(WeatherContext context, User user, City city)
        {
            context.Users.Attach(user);
            context.Cities.Attach(city);

            user.Cities.Remove(city);
            city.Users.Remove(user);

            context.SaveChanges();
        }

        public List<SearchHistoryRecord> GetSearchHistoryRecords(WeatherContext context, User user)
        {
            context.Users.Attach(user);
            return user.SearchHistoryRecords.ToList();
        }

        public void AddToHistory(WeatherContext context, User user, City city)
        {
            context.Users.Attach(user);
            context.Cities.Attach(city);

            var record = new SearchHistoryRecord { User = user, City = city, DateTime = DateTime.Now};

            context.SearchHistoryRecords.Add(record);
            user.SearchHistoryRecords.Add(record);

            context.SaveChanges();
        }
    }
}
