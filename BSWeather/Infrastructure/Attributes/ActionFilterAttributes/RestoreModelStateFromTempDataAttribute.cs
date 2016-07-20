using System.Web.Mvc;

namespace BSWeather.Infrastructure.Attributes.ActionFilterAttributes
{
    public class RestoreModelStateFromTempDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.Controller.TempData.ContainsKey("ModelState"))
            {
                filterContext.Controller.ViewData.ModelState.Merge(
                    (ModelStateDictionary)filterContext.Controller.TempData["ModelState"]);
            }
        }
    }
}
