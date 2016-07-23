using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
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
                using (var context = DependencyResolver.Current.GetService<WeatherContext>())
                {
                    return context.Cities.ToList().GetRange(0, 5);
                }
            }
        }

        public async Task<City> TrackCityAsync(int cityId, string cityName)
        {
            using (var context = DependencyResolver.Current.GetService<WeatherContext>())
            {
                var city = await context.Cities.FirstOrDefaultAsync(c => c.ExternalIdentifier == cityId);
                if (city != null)
                {
                    return city;
                }
                var newCity = new City
                {
                    ExternalIdentifier = cityId,
                    Name = cityName
                };
                context.Cities.Add(newCity);
                await context.SaveChangesAsync();
                return newCity;
            }
        }

        public async Task AddToFavouritesAsync(WeatherContext context, User user, City city)
        {
            context.Users.Attach(user);
            context.Cities.Attach(city);

            if (user.Cities.Count < 6)
            {
                user.Cities.Add(city);
                city.Users.Add(user);
            }

            await context.SaveChangesAsync();
        }

        public async Task RemoveFromFavouritesAsync(WeatherContext context, User user, City city)
        {
            context.Users.Attach(user);
            context.Cities.Attach(city);

            user.Cities.Remove(city);
            city.Users.Remove(user);

            await context.SaveChangesAsync();
        }

        public async Task AddToHistoryAsync(WeatherContext context, User user, City city)
        {
            context.Users.Attach(user);
            context.Cities.Attach(city);

            var record = new SearchHistoryRecord
            {
                User = user,
                City = city,
                DateTime = DateTime.Now
            };

            context.SearchHistoryRecords.Add(record);
            user.SearchHistoryRecords.Add(record);

            await context.SaveChangesAsync();
        }

        public List<SearchHistoryRecord> GetSearchHistoryRecords(WeatherContext context, User user)
        {
            context.Users.Attach(user);
            return user.SearchHistoryRecords.ToList();
        }
    }
}