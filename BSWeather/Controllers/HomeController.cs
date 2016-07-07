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
using BSWeather.Services;
using Newtonsoft.Json;

namespace BSWeather.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? id)
        {
            ViewData["Weather"] = new OpenWeatherService().GetWeather(id ?? 703448);

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