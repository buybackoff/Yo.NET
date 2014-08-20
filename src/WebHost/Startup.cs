using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

[assembly: OwinStartup(typeof(WebHost.Startup))]

namespace WebHost {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            
            // Wire-in the authentication middleware
            ConfigureAuth(app);

            app.Map("/signalr", appSr => {
                appSr.MapSignalR("", new HubConfiguration());
                //GlobalHost.HubPipeline.RequireAuthentication();
            });

            app.Map("/api", appApi => {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                app.UseCors(CorsOptions.AllowAll);
                appApi.UseWebApi(config);
            });


            var fsOptions = new FileServerOptions() {
                EnableDirectoryBrowsing = false,
#if DEBUG
                FileSystem = new PhysicalFileSystem(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FrontEnd/app/")),
#else
                FileSystem = new PhysicalFileSystem(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FrontEnd/dist/")),
#endif
            };
            var staticMiddleWare = new StaticFileMiddleware(environment => Task.FromResult(0), fsOptions.StaticFileOptions);

            // Angular html5mode support
            // Must be before UseFileServer
            // We then check if there is 404 error and return index.html instead
            app.Use(async (context, next) => {
                await next(); // FileServer
                IOwinResponse res = context.Response;
                if (res.StatusCode == 404) {
                    context.Request.Path = new PathString("/index.html");
                    await staticMiddleWare.Invoke(context.Environment);
                }
            });
            app.UseFileServer(false);


            

            
        }
    }
}
