using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSWeather.Infrastructure;
using BSWeather.Infrastructure.ActionFilterAttributes;
using BSWeather.Infrastructure.Attributes.ActionFilterAttributes;
using BSWeather.Infrastructure.Attributes.AuthorizeAttributes;
using BSWeather.Infrastructure.Context;
using BSWeather.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebGrease.Css.Extensions;
using DependencyResolver = System.Web.Mvc.DependencyResolver;

namespace BSWeather.Controllers
{
    public class UserController : Controller
    {
        public UserManager UserManager => Request.GetOwinContext().GetUserManager<UserManager>();

        public SignInManager SignInManager => Request.GetOwinContext().Get<SignInManager>();
        
        [AnonymousOnly]
        [RestoreModelStateFromTempData]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AnonymousOnly]
        [ValidateAntiForgeryToken]
        [SetTempDataModelState]
        public ActionResult Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                    Request.GetOwinContext().Authentication.SignIn(
                        new AuthenticationProperties { IsPersistent = false },
                        identity
                    );

                    using (var context = DependencyResolver.Current.GetService<WeatherContext>())
                    {
                        context.Users.Attach(user);
                        context.Cities.Take(5).ForEach(user.Cities.Add);
                        context.SaveChanges();
                    }

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return RedirectToAction("Index");
        }
        
        [AnonymousOnly]
        public ActionResult SignIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(model.PreviousUrl);
            }

            var user = UserManager.Find(model.Email, model.Password);
            if (user != null)
            {
                if (!UserManager.IsEmailConfirmed(user.Id))
                {
                    //...
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = SignInManager.PasswordSignIn(model.Email, model.Password, shouldLockout: false, isPersistent: false);
            switch (result)
            {
                //TODO: EXTEND LOGICS
                case SignInStatus.Success:
                    return Redirect(model.PreviousUrl);
                case SignInStatus.LockedOut:
                    return Redirect(model.PreviousUrl);
                case SignInStatus.RequiresVerification:
                    return Redirect(model.PreviousUrl);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Возникла ошибка входа.");
                    return Redirect(model.PreviousUrl);
            }
        }

        [RedirectingAuthorize]
        public ActionResult SignOut(string previousUrl)
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect(previousUrl);
        }
    }
}