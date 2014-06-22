using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ServiceImplementations.Common.Startup))]

namespace ServiceImplementations.Common {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new CustomUserIdProvider());          
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
