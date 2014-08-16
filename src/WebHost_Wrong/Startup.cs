using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Yo.WebHost;
using Yo.WebHost.Providers;


[assembly: OwinStartup(typeof(Startup))]

namespace Yo.WebHost {

    public class Startup {

        //public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        //public static GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; private set; }
        //public static FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app) {
            app.UseCors(CorsOptions.AllowAll);
            
            ConfigureOAuth(app);

            app.MapSignalR();
            app.Map("/api", appApi => {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                appApi.UseWebApi(config);
                }
                );
            app.UseFileServer(false);
        }

        public void ConfigureOAuth(IAppBuilder app) {
            
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            
            var oAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            var oAuthServerOptions = new OAuthAuthorizationServerOptions {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(oAuthBearerOptions);

            //Configure Google External Login
            var googleAuthOptions = new GoogleOAuth2AuthenticationOptions() {
                ClientId = "xxxxxx",
                ClientSecret = "xxxxxx",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            var facebookAuthOptions = new FacebookAuthenticationOptions() {
                AppId = "xxxxxx",
                AppSecret = "xxxxxx",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

        }
    
        
    }

    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

        }
    }



}