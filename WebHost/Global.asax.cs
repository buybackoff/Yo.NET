using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using ServiceImplementations.Common;

[assembly: OwinStartup(typeof(WebHost.Startup))]

namespace WebHost {
    public class Application : System.Web.HttpApplication {
        protected void Application_Start() {

        }

    }

    public class Startup {
        public void Configuration(IAppBuilder app) {
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new CustomUserIdProvider());
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();

            AppStarter.Start();
        }
    }

}
