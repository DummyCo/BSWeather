using System.Web.Mvc;

namespace BSWeather.Infrastructure.Attributes.ActionFilterAttributes
{
    public class SetTempDataModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            filterContext.Controller.TempData["ModelState"] =
               filterContext.Controller.ViewData.ModelState;
        }
    }
}
