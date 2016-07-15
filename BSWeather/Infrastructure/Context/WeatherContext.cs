using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSWeather.Models;

namespace BSWeather.Infrastructure.Context
{
    public class WeatherContext : DbContext
    {
        public DbSet<City> Cities { get; set; }

        public WeatherContext() : base("WeatherContext")
        {
            Database.SetInitializer(new WeatherContexInitializer());
        }
    }
}
