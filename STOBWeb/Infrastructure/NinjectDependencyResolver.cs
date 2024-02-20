using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using STOBBusinessLayer.Services;
using STOBBusinessLayer.Services.Interfaces;

namespace STOBWeb.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
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
            _kernel.Bind<IReferenceService>().To<ReferenceService>();
            _kernel.Bind<IErrorService>().To<ErrorService>();
        }
    }
}