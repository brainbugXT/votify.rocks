using System;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
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
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (resolver == null) throw new ArgumentNullException("resolver");
            _app = app;
            _configuration = configuration;
            _resolver = resolver;
        }

        public void Start()
        {
            Debugger.Break();
            _configuration.Resolver = _resolver;
            _app.MapSignalR(_configuration);
        }
    }
}
