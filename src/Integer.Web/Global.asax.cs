using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Repository;
using Integer.Infrastructure.Events;
using Integer.Domain.Agenda;
using Raven.Client;
using Integer.Infrastructure.Email;
using System.Text;
using Integer.Infrastructure.IoC;
using Integer.Domain.Acesso;
using Integer.Infrastructure.Tasks;

namespace Integer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allsvg}", new { allsvg = @".*\.svg(/.*)?" });
            routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });

            routes.MapRoute(
                "Login", 
                "login",
                new { controller = "Usuario", action = "Login" } 
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Calendario", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}