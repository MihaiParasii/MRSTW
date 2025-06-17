// App_Start/RouteConfig.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OtdamDarom.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // ACEASTA RUTA ESTE ABSOLUT NECESARA SI TREBUIE SA FIE PRIMA!
            routes.MapRoute(
                name: "CategoryById", // Numele rutei, folosit in Url.RouteUrl
                url: "Category/{id}", // Modelul URL-ului, va corespunde cu /Category/1, /Category/2 etc.
                defaults: new { controller = "Home", action = "CategoryDetails", id = UrlParameter.Optional }
            );

            // Ruta Default trebuie sa fie DUPA rutele mai specifice
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}