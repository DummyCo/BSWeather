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
            OpenWeatherBase.RootObject weather = null;
            var service = new OpenWeatherService();
            if (ModelState.IsValid)
            {
                weather = service.GetWeatherByCityName(citySearch.CityName, actualDays);
            }
            
            //Snippet to return default weather if smth went wrong
            //if (weather == null)
            //{
            //    weather = service.GetWeatherById(703448, actualDays);
            //}

            ViewData["Weather"] = weather;
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