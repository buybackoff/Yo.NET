using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(WebHost.Startup))]

namespace WebHost {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            
            // Wire-in the authentication middleware
            ConfigureAuth(app);

            app.MapSignalR();
            GlobalHost.HubPipeline.RequireAuthentication();

            app.Map("/api", appApi => {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                app.UseCors(CorsOptions.AllowAll);
                appApi.UseWebApi(config);
            }
                );
            //app.UseFileServer(false);
        }
    }
}
