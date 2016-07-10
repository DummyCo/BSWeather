using System.Web.Mvc;
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
            ViewData["Weather"] = DependencyResolver.Current.GetService<OpenWeatherService>().GetWeatherById(id, days);
            ViewData["Days"] = days;

            return View();
        }

        [HttpPost]
        public ActionResult SearchCityByName(CitySearch citySearch)
        {
            _logger.Info($"SearchCityByName called for {citySearch.CityName} city and {citySearch.Days} days");
            int actualDays = citySearch.Days;
            OpenWeatherBase.RootObject weather = null;
            var service = new OpenWeatherService();
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

            ViewData["Weather"] = weather;
            ViewData["Days"] = actualDays;

            return View("Index");
        }
    }
}