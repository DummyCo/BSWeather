using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSWeather.Infrastructure;
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
        private UserManager _userManager;
        private WeatherContext _weatherContext;

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public WeatherContext Context
        {
            get
            {
                return _weatherContext ?? Request.GetOwinContext().Get<WeatherContext>();
            }
            private set
            {
                _weatherContext = value;
            }
        }

        public HomeController(ILogger logger)
        {
            _logger = logger;
            logger.Info("HomeController ctor");
        }

        public ActionResult Index(int id, int days)
        {
            _logger.Info($"Index called with {id} id for {days} days");

            var weather = DependencyResolver.Current.GetService<OpenWeatherService>().GetWeatherById(id, days);
            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            bsWeatherService.AddToHistory(weather.City.Name);

            var user = UserManager.FindById(User.Identity.GetUserId());
            ViewData["Weather"] = weather;
            ViewData["Days"] = days;
            if (user != null)
            {
                ViewData["FavouriteCities"] = user.Cities.ToList();
            }
            else
            {
                ViewData["FavouriteCities"] = bsWeatherService.DeafultFavouriteCities;
            }

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

            var user = UserManager.FindById(User.Identity.GetUserId());
            ViewData["Weather"] = weather;
            ViewData["Days"] = citySearch.Days;
            if (user != null) { 
                ViewData["FavouriteCities"] = user.Cities.ToList();
            }
            else
            {
                ViewData["FavouriteCities"] = bsWeatherService.DeafultFavouriteCities;
            }

            return View("Index");
        }

        public ActionResult AddToFavourites(int id, string cityName, int days)
        {
            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            var city = bsWeatherService.TrackCity(id, cityName);

            var user = UserManager.FindById(User.Identity.GetUserId());

            Context.Users.Attach(user);
            Context.Cities.Attach(city);

            if (user.Cities.Count < 6)
            {
                user.Cities.Add(city);
                city.Users.Add(user);
            }

            Context.SaveChanges();

            return RedirectToAction("SearchCityByName", new CitySearch { CityName = cityName, Days = days });
        }

        public ActionResult RemoveFromFavourites(int id, string cityName, int days)
        {
            var bsWeatherService = DependencyResolver.Current.GetService<BsWeatherService>();
            var city = bsWeatherService.TrackCity(id, cityName);
            var user = UserManager.FindById(User.Identity.GetUserId());

            Context.Users.Attach(user);
            Context.Cities.Attach(city);

            user.Cities.Remove(city);
            city.Users.Remove(user);

            Context.SaveChanges();

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