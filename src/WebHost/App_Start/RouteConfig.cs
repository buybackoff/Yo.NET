using System.Web.Mvc;
using System.Web.Routing;

namespace WebHost {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Manage",
                url: "account/manage/{action}/{id}",
                defaults: new { controller = "Manage", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "RolesAdmin",
                url: "account/RolesAdmin/{action}/{id}",
                defaults: new { controller = "RolesAdmin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "UsersAdmin",
                url: "account/UsersAdmin/{action}/{id}",
                defaults: new { controller = "UsersAdmin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Account",
                url: "account/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );

            

        }
    }
}
