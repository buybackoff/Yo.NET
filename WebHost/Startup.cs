using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using ServiceImplementations;

[assembly: OwinStartup(typeof(WebHost.Startup))]

namespace WebHost {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            // Order is important!!! (was't that obvious!????)
            AppStarter.Start();
            // CustomUserIdProvider depends on Request indirectly via SessionFeature TODO check this
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new CustomUserIdProvider());
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
            
        }
    }
}