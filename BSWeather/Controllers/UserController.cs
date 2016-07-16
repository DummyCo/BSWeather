using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSWeather.Infrastructure;
using BSWeather.Infrastructure.ActionFilterAttributes;
using BSWeather.Infrastructure.Context;
using BSWeather.Models;
using BSWeather.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebGrease.Css.Extensions;
using DependencyResolver = System.Web.Mvc.DependencyResolver;

namespace BSWeather.Controllers
{
    public class UserController : Controller
    {
        private UserManager _userManager;

        private SignInManager _signInManager;

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public SignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<SignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: Registration
        [RestoreModelStateFromTempData]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

                    using (var context = new WeatherContext())
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

        public ActionResult SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.Find(model.Email, model.Password);
                if (user != null)
                {
                    if (!UserManager.IsEmailConfirmed(user.Id))
                    {
                        //...
                    }
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = SignInManager.PasswordSignIn(model.Email, model.Password, shouldLockout: false, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return Redirect(model.PreviousUrl.Replace("User", string.Empty)); //TODO: WTF :D
                case SignInStatus.LockedOut:
                    return Redirect(model.PreviousUrl);
                case SignInStatus.RequiresVerification:
                    return Redirect(model.PreviousUrl);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return Redirect(model.PreviousUrl);
            }
        }

        public ActionResult SignOut(string previousUrl)
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect(previousUrl);
        }



    }
}