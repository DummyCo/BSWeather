using System.Web.Mvc;
using BSWeather.Models;
using BSWeather.Services;

namespace BSWeather.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? id)
        {
            ViewData["Weather"] = new OpenWeatherService().GetWeatherById(id ?? 703448);

            return View();
        }

        [HttpPost]
        public ActionResult SearchCityByName(CitySearch citySearch)
        {
            if (ModelState.IsValid)
            {
                ViewData["Weather"] = new OpenWeatherService().GetWeatherByCityName(citySearch.CityName);
            }
            else
            {
                ViewData["Weather"] = new OpenWeatherService().GetWeatherById(703448);
            }
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