using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BSWeather.Infrastructure.Context;
using BSWeather.Services;
using BSWeather.Services.Logger;
using Ninject;

namespace BSWeather.Infrastructure
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public DependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            _kernel.Settings.InjectNonPublic = true;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<ILogger>().To<ThreadSafeLogger>().InSingletonScope();
            _kernel.Bind<ILogPrinter>().To<DebugLogPrinter>().InSingletonScope();
            _kernel.Bind<OpenWeatherService>().ToSelf().InTransientScope();
            _kernel.Bind<BsWeatherService>().ToSelf().InTransientScope();
            _kernel.Bind<AvailabilityCheckService>().ToSelf().InTransientScope();
            _kernel.Bind<WeatherContext>().ToSelf().InTransientScope();
        }
    }
}
