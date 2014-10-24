using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReferencingSystem.Application.Main.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Reference",
               url: "Reference/{action}/{TransId}",
               defaults: new { controller = "Reference", action = "Index" }
           );
            routes.MapRoute(
                name: "ReferenceFileAction",
                url: "Reference/{action}/{TransId}/{Id}",
                defaults: new { controller = "Reference", action = "File" }
            );
        }
    }
}
