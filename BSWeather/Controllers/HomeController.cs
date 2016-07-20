using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSWeather.Infrastructure;
using BSWeather.Infrastructure.Attributes.ActionFilterAttributes;
using BSWeather.Infrastructure.Context;
using BSWeather.Models;
using BSWeather.Services;
using BSWeather.Services.Logger;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DependencyResolver = System.Web.Mvc.DependencyResolver;

namespace BSWeather.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public UserManager UserManager => Request.GetOwinContext().GetUserManager<UserManager>();

        public WeatherContext Context => Request.GetOwinContext().Get<WeatherContext>();

        public HomeController(ILogger logger)
        {
            _logger = logger;
            logger.Info("HomeController ctor");
        }

        [RestoreModelStateFromTempData]
        public ActionResult Index(int id, int days)
        {
            _logger.Info($"Index called with {id} id for {days} days");

            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();

            City city = null;
            OpenWeatherBase.RootObject weather = null;

            if (id != 0)
            {
                weather = DependencyResolver.Current.GetService<OpenWeatherService>().GetWeatherById(id, days);

                if (weather != null)
                {
                    city = bsWeatherService.TrackCity(id, weather.City.Name);
                }
            }

            var user = UserManager.FindById(User.Identity.GetUserId());

            ViewData["Weather"] = weather;
            ViewData["Days"] = days;

            if (user != null)
            {
                if (city != null)
                {
                    bsWeatherService.AddToHistory(Context, user, city);
                }
                ViewData["FavouriteCities"] = user.Cities.ToList();
            }
            else
            {
                ViewData["FavouriteCities"] = bsWeatherService.DeafultFavouriteCities;
            }

            return View();
        }

        [RestoreModelStateFromTempData]
        [SetTempDataModelState]
        public ActionResult SearchCityByName(CitySearch citySearch)
        {
            _logger.Info($"SearchCityByName called for {citySearch.CityName} city and {citySearch.Days} days");

            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            var openWeatherService = DependencyResolver.Current.GetService<OpenWeatherService>();
            var user = UserManager.FindById(User.Identity.GetUserId());

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
            else if (user != null)
            {
                var city = bsWeatherService.TrackCity(weather.City.Id, weather.City.Name);
                bsWeatherService.AddToHistory(Context, user, city);
            }

            return RedirectToAction("Index", new { id = weather?.City.Id ?? 0, citySearch.Days });
        }

        [RedirectingAuthorize]
        public ActionResult AddToFavourites(int id, string cityName, int days)
        {
            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            var city = bsWeatherService.TrackCity(id, cityName);
            var user = UserManager.FindById(User.Identity.GetUserId());

            bsWeatherService.AddToFavourites(Context, user, city);

            return RedirectToAction("Index", new { id, days });
        }

        [RedirectingAuthorize]
        public ActionResult RemoveFromFavourites(int id, string cityName, int days)
        {
            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            var city = bsWeatherService.TrackCity(id, cityName);
            var user = UserManager.FindById(User.Identity.GetUserId());

            bsWeatherService.RemoveFromFavourites(Context, user, city);

            return RedirectToAction("Index", new { id, days });
        }

        [RedirectingAuthorize]
        [RestoreModelStateFromTempData]
        public ActionResult SearchHistroy()
        {
            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            var user = UserManager.FindById(User.Identity.GetUserId());

            var records = bsWeatherService.GetSearchHistoryRecords(Context, user);
            records.Reverse();

            ViewData["SearchHistoryRecords"] = records.Take(20).ToList();
            return View("SearchHistroy");
        }
    }
}