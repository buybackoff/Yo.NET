using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using Owin;
using Yo.WebHost;

[assembly: OwinStartup(typeof(Startup))]

namespace Yo.WebHost {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            // Order is important!!! (was't that obvious!????)
            // CustomUserIdProvider depends on Request indirectly via SessionFeature TODO check this
            // Any connection or hub wire up and configuration should go here
            //var transportManager = GlobalHost.DependencyResolver.Resolve<ITransportManager>() as TransportManager;
            // fixed issue with web sockets in SS 4.0.23 (just replaced one dll in packages)
            //transportManager.Remove("websockets");
            app.MapSignalR();
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            
            
        }
    }

    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "_/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

        }
    }
}