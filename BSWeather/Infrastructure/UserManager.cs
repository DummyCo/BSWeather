using BSWeather.Infrastructure.Context;
using BSWeather.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace BSWeather.Infrastructure
{
    public class UserManager : UserManager<User>
    {
        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            var manager = new UserManager(new UserStore<User>(context.Get<WeatherContext>()));

            // ...

            return manager;
        }

        public UserManager(IUserStore<User> store) : base(store)
        {
        }
    }
}
