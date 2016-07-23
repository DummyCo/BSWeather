using System;
using System.Web.Mvc;

namespace BSWeather.Infrastructure.Attributes.ActionFilterAttributes
{
    public class RedirectingAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                const string loginUrl = "/";
                filterContext.Result = new RedirectResult(loginUrl);
            }
        }
    }
}