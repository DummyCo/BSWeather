using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        }
    }
}
