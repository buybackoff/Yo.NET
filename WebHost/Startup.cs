using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Services;

[assembly: OwinStartup(typeof(WebHost.Startup))]

namespace WebHost {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            // Order is important!!! (was't that obvious!????)
            AppStarter.Start();
            AppDomain.CurrentDomain.Load(typeof(YoHub).Assembly.FullName);
            // CustomUserIdProvider depends on Request indirectly via SessionFeature TODO check this
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new CustomUserIdProvider());
            // Any connection or hub wire up and configuration should go here
            //var transportManager = GlobalHost.DependencyResolver.Resolve<ITransportManager>() as TransportManager;
            // fixed issue with web sockets in SS 4.0.23 (just replaced one dll in packages)
            //transportManager.Remove("websockets");
            app.MapSignalR();
            
        }
    }
}