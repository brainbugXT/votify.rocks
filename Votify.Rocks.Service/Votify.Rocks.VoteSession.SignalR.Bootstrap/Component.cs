using Microsoft.Practices.Unity;
using Serko.Wiring;

namespace Votify.Rocks.VoteSession.SignalR.Bootstrap
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
            _container.RegisterType<ISignalRVoteSessionBroadcaster, SignalRVoteSessionBroadcaster>();
        }
    }
}
