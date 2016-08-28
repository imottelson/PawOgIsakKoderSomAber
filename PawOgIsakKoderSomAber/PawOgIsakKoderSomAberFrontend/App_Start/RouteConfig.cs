using System.Web.Mvc;
using System.Web.Routing;

namespace PawOgIsakKoderSomAberFrontend
{
    public class RouteConfig
    {
        public static void Configure(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "BeginRequest", id = UrlParameter.Optional }
            );
        }
    }
}