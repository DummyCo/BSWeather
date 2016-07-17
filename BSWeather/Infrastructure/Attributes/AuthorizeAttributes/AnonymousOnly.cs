using System.Web.Mvc;
using System.Web.Routing;

namespace BSWeather.Infrastructure.Attributes.AuthorizeAttributes
{
    public class AnonymousOnly : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
        }
    }
}
