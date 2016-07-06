using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using BSWeather.Models;
using Newtonsoft.Json;

namespace BSWeather.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?q=Kiev&APPID=" + WebConfigurationManager.AppSettings["OpenWeatherMapAPIKEY"];
            var webClient = new WebClient();
            var resultString = webClient.DownloadString(url);

            var weather = JsonConvert.DeserializeObject<WeatherBase.RootObject>(resultString);
            ViewData["Weather"] = weather;

            return View();
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