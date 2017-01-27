using System;
using Microsoft.Practices.Unity;
using Serko.Wiring;

namespace Votify.Rocks.Service.Bootstrap
{
    public class Component : IComponent
    {
        private readonly IUnityContainer _container;
        private readonly ISettings _settings;

        public Component(IUnityContainer container, ISettings settings)
        {
            _container = container;
            _settings = settings;
        }

        public void Register()
        {
            var maxParticipants = int.Parse(_settings.GetValue("MaxParticipants"));
            var sendGridApiKey = _settings.GetValue("SendGridApiKey");
            var sessionCacheExpiryHours = _settings.GetValue("SessionCacheExpiryHours");
            var voteSessionStoreConnectionString = _settings.GetValue("VoteSessionStoreConnectionString");
            var voteSessionTableName = _settings.GetValue("VoteSessionTableName");
            _container.RegisterType<ICacheObject, MemoryCacheObject>(new InjectionConstructor(TimeSpan.FromSeconds(double.Parse(sessionCacheExpiryHours))));
            _container.RegisterType<IVoteSessionStore, VoteSessionStore>(new InjectionConstructor(voteSessionStoreConnectionString, voteSessionTableName));
            _container.RegisterType<IVoteSessionStoreService, VoteSessionStoreService>(new InjectionConstructor(new ResolvedParameter<IVoteSessionStore>()));
            _container.RegisterType<IVoteSessionService, VoteSessionService>(new InjectionConstructor(new ResolvedParameter<IVoteSessionStoreService>(), new ResolvedParameter<ICacheObject>(), maxParticipants));
            _container.RegisterType<IResourceTextReader, EmailTemplateResourceReader>();
            _container.RegisterType<ISendMailService, SendMailService>(new InjectionConstructor(new ResolvedParameter<IVoteSessionService>(), new ResolvedParameter<IResourceTextReader>(), sendGridApiKey));
            _container.RegisterType<IRandomNameGeneratorService, RandomNameGeneratorService>();
        }
    }
}
