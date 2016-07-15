﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSWeather.Models;

namespace BSWeather.Infrastructure.Context
{
    public class WeatherContexInitializer : DropCreateDatabaseIfModelChanges<WeatherContext>
    {
        protected override void Seed(WeatherContext context)
        {
            base.Seed(context);

            var defaultCities = new List<City>
            {
                new City {Name = "Kiev", ExternalIdentifier = 703448},
                new City {Name = "Lviv", ExternalIdentifier = 702550}
            };

            context.Cities.AddRange(defaultCities);
        }
    }
}
