using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace BSWeather
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{days}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = WebConfigurationManager.AppSettings["DefaultCityId"],
                    days = WebConfigurationManager.AppSettings["DefaultDaysCount"]
                }
            );
        }
    }
}
