using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using Serko.Wiring;

namespace Votify.Rocks.Owin.SignalR
{
    [Startup]
    public class Startup : IStartup
    {
        private readonly IAppBuilder _app;
        private readonly HubConfiguration _configuration;
        private readonly IDependencyResolver _resolver;

        public Startup(IAppBuilder app, HubConfiguration configuration, IDependencyResolver resolver)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            _app = app;
            _configuration = configuration;
            _resolver = resolver;
        }

        public void Start()
        {
            _configuration.Resolver = _resolver;
            _configuration.EnableJSONP = true;
            _app.UseCors(CorsOptions.AllowAll);
            _app.MapSignalR(_configuration);
        }
    }
}
