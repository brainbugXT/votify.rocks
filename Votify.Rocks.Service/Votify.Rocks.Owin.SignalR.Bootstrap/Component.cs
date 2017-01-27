using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Serko.Wiring;

namespace Votify.Rocks.Owin.SignalR.Bootstrap
{
    public class Component : IComponent
    {
        private readonly IUnityContainer _container;

        public Component(IUnityContainer container)
        {
            _container = container;
        }

        public void Register()
        {
            _container.RegisterType<IStartup, Startup>(typeof(Startup).FullName);
            _container.RegisterType<HubConfiguration>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDependencyResolver, SignalRUnityDependencyResolver>(new ContainerControlledLifetimeManager());
        }
    }
}
