using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSWeather.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BSWeather.Infrastructure.Context
{
    public class WeatherContext : IdentityDbContext<User>
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<SearchHistoryRecord> SearchHistoryRecords { get; set; }

        public WeatherContext() : base("WeatherContext")
        {
            Database.SetInitializer(new WeatherContexInitializer());
        }
    }
}
