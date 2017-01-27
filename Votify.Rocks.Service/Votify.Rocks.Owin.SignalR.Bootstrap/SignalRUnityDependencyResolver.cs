using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Serko.Services.Owin.SignalR.Bootstrap.Properties;
using ServiceList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.Type, System.Func<object>>>;
using ServicesList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.Type, System.Collections.Generic.IEnumerable<System.Func<object>>>>;
using ServiceItem = System.Collections.Generic.KeyValuePair<System.Type, System.Func<object>>;
using ServicesItem = System.Collections.Generic.KeyValuePair<System.Type, System.Collections.Generic.IEnumerable<System.Func<object>>>;

namespace Votify.Rocks.Owin.SignalR.Bootstrap
{
    // The SignalR resolver only has one set of registrations, so a single type registration 
    // is the same as multiple type registrations, but with only one item.
    //
    // Unity works differently and instead has the concept of a single (unnamed) registration 
    // and multiple (named) registrations.
    // 
    // The SignalR resolver can register a single type and later resolve it with the multiple 
    // type resolver. To simulate this behaviour in Unity we always register with a name and 
    // use ResolveAll, even to resolve a single type registration.
    public sealed class SignalRUnityDependencyResolver : DefaultDependencyResolver
    {
        // The base class (DefaultDependencyResolver) calls Register for each of it's 
        // default service registrations in it's constructor.
        //
        // The serviceList and servicesList capture these base class registrations.
        // This is done because the Unity container isn't assigned (i.e. it's null)
        // until the base class completes it's construction.
        //
        // After the Unity container is assigned we re-run the registrations into the 
        // Unity container.
        private readonly ServiceList _serviceList = new ServiceList();
        private readonly ServicesList _servicesList = new ServicesList();

        private readonly IUnityContainer _container;

        public SignalRUnityDependencyResolver(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
            foreach (var service in _serviceList)
            {
                Register(service.Key, service.Value);
            }
            foreach (var services in _servicesList)
            {
                Register(services.Key, services.Value);
            }
            _serviceList.Clear();
            _servicesList.Clear();
        }

        // Here we mimic the DefaultDependencyResolver behaviour that allows multiple 
        // registrations, but complains if it finds more than one during a single 
        // type resolve.
        public override object GetService(Type serviceType)
        {
            var services = _container.ResolveAll(serviceType).ToList();
            if (services.Count > 1)
            {
                throw new InvalidOperationException(string.Format(Resources.MoreThanOneServiceRegistered, serviceType.FullName));
            }
            return services.Count == 0
                ? null
                : services[0];
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var services = _container.ResolveAll(serviceType).ToList();
            return services.Count == 0
                ? null
                : services;
        }

        // For single registrations we use a fixed name (the type's fullname) so that it's 
        // overwritten by subsequent registrations for the same type.
        //
        // We also register an unnamed type so normal Unity resolve works as expected.
        public override void Register(Type serviceType, Func<object> activator)
        {
            if (_container == null)
            {
                _serviceList.Add(new ServiceItem(serviceType, activator));
                return;
            }
            _container.RegisterType(serviceType,
                serviceType.FullName,
                new InjectionFactory(c => activator())
                );
            _container.RegisterType(serviceType,
                new InjectionFactory(c => activator())
                );
        }

        // For multiple registrations we use a unique name (a guid) so that the type 
        // registration is always added to the existing list.
        public override void Register(Type serviceType, IEnumerable<Func<object>> activators)
        {
            if (_container == null)
            {
                _servicesList.Add(new ServicesItem(serviceType, activators));
                return;
            }
            foreach (var activatorIterator in activators)
            {
                var activator = activatorIterator;
                _container.RegisterType(serviceType,
                    Guid.NewGuid().ToString(),
                    new InjectionFactory(c => activator())
                    );
            }
        }
    }
}
