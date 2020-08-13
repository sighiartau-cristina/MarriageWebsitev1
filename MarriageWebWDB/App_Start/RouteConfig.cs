using System.Web.Mvc;
using System.Web.Routing;

namespace MarriageWebWDB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Error",                                           // Route name
                url : "{controller}/{action}/{errorMessage}",                            // URL with parameters
                defaults: new { controller = "Error", action = "Index", errorMessage = UrlParameter.Optional }  // Parameter defaults
            );
        }
    }
}
