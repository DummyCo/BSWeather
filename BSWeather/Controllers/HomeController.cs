using System.Web.Mvc;
using BSWeather.Models;
using BSWeather.Services;

namespace BSWeather.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int id, int days)
        {
            ViewData["Weather"] = new OpenWeatherService().GetWeatherById(id, days);
            ViewData["Days"] = days;

            return View();
        }

        [HttpPost]
        public ActionResult SearchCityByName(CitySearch citySearch)
        {
            int actualDays = citySearch.Days;
            if (ModelState.IsValid)
            {
                ViewData["Weather"] = new OpenWeatherService().GetWeatherByCityName(citySearch.CityName, actualDays);
            }
            else
            {
                ViewData["Weather"] = new OpenWeatherService().GetWeatherById(703448, actualDays);
            }
            ViewData["Days"] = actualDays;

            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}