using System.Collections.Generic;
using System.Data.Entity;
using BSWeather.Models;

namespace BSWeather.Infrastructure.Context
{
    public class WeatherContexInitializer : DropCreateDatabaseIfModelChanges<WeatherContext>
    {
        protected override void Seed(WeatherContext context)
        {
            base.Seed(context);

            context.Configuration.ProxyCreationEnabled = true;
            context.Configuration.LazyLoadingEnabled = true;

            var defaultCities = new List<City>
            {
                new City {Name = "Kiev", ExternalIdentifier = 703448},
                new City {Name = "Lviv", ExternalIdentifier = 702550},
                new City {Name = "Kharkiv", ExternalIdentifier = 706483},
                new City {Name = "Dnipropetrovsk", ExternalIdentifier = 709930},
                new City {Name = "Odessa", ExternalIdentifier = 698740}
            };

            context.Cities.AddRange(defaultCities);
        }
    }
}