using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BSWeather.Infrastructure.Context;
using BSWeather.Models;
using BSWeather.Services;
using BSWeather.Services.Logger;

namespace BSWeather.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger;
            logger.Info("HomeController ctor");
        }

        public ActionResult Index(int id, int days)
        {
            List<City> favouriteCities;
            using (var context = new WeatherContext())
            {
                favouriteCities = context.Cities.ToList();
            }

            _logger.Info($"Index called with {id} id for {days} days");
            ViewData["Weather"] = DependencyResolver.Current.GetService<OpenWeatherService>().GetWeatherById(id, days);
            ViewData["Days"] = days;
            ViewData["FavouriteCities"] = favouriteCities;

            return View();
        }

        //[HttpPost]
        public ActionResult SearchCityByName(CitySearch citySearch)
        {
            _logger.Info($"SearchCityByName called for {citySearch.CityName} city and {citySearch.Days} days");
            int actualDays = citySearch.Days;
            OpenWeatherBase.RootObject weather = null;
            var service = DependencyResolver.Current.GetService<OpenWeatherService>();
            if (ModelState.IsValid)
            {
                weather = service.GetWeatherByCityName(citySearch.CityName, actualDays);
            }
            else
            {
                _logger.Warning($"SearchCityByName invalid ModelState");
            }

            //Snippet to return default weather if smth went wrong
            if (weather == null)
            {
                _logger.Warning($"SearchCityByName returned null weather");
            }
            else
            {
                using (var context = new WeatherContext())
                {
                    bool contains = context.Cities.Any(city => city.ExternalIdentifier == weather.City.Id);
                    if (!contains)
                    {
                        context.Cities.Add(new City
                        {
                            ExternalIdentifier = weather.City.Id,
                            Name = weather.City.Name
                        });
                        context.SaveChanges();
                    }
                }
            }

            List<City> favouriteCities;
            using (var context = new WeatherContext())
            {
                favouriteCities = context.Cities.ToList();
            }

            ViewData["Weather"] = weather;
            ViewData["Days"] = actualDays;
            ViewData["FavouriteCities"] = favouriteCities;

            return View("Index");
        }
    }
}