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
            _logger.Info($"Index called with {id} id for {days} days");

            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            var weather = DependencyResolver.Current.GetService<OpenWeatherService>().GetWeatherById(id, days);

            bsWeatherService.FillViewData(
                ViewData,
                weather,
                days
            );

            bsWeatherService.AddToHistory(weather.City.Name);

            return View();
        }

        //[HttpPost]
        public ActionResult SearchCityByName(CitySearch citySearch)
        {
            _logger.Info($"SearchCityByName called for {citySearch.CityName} city and {citySearch.Days} days");

            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            var openWeatherService = DependencyResolver.Current.GetService<OpenWeatherService>();

            OpenWeatherBase.RootObject weather = null;
            if (ModelState.IsValid)
            {
                weather = openWeatherService.GetWeatherByCityName(citySearch.CityName, citySearch.Days);
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
                bsWeatherService.AddToHistory(weather.City.Name);
            }

            bsWeatherService.FillViewData(ViewData, weather, citySearch.Days);

            return View("Index");
        }

        public ActionResult AddToFavourites(int id, string cityName, int days)
        {
            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            bsWeatherService.AddToFavourites(id, cityName);

            return RedirectToAction("SearchCityByName", new CitySearch { CityName = cityName, Days = days} );
        }

        public ActionResult RemoveFromFavourites(int id, string cityName, int days)
        {
            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            bsWeatherService.RemoveFromFavourites(id, cityName);

            return RedirectToAction("Index", new { days });
        }

        public ActionResult SearchHistroy()
        {
            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            var records = bsWeatherService.GetSearchHistoryRecords();
            records.Reverse();

            ViewData["SearchHistoryRecords"] = records.Take(20).ToList();
            return View("SearchHistroy");
        }
    }
}