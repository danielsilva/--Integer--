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
using Integer.Domain.Services;
using Integer.Domain.Agenda;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace Integer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string RavenSessionKey = "RavenDB.Session";
        
        public static IDocumentSession CurrentSession
        {
            get 
            { 
                return (IDocumentSession)HttpContext.Current.Items[RavenSessionKey]; 
            }
            set 
            {
                HttpContext.Current.Items[RavenSessionKey] = value;
            }
        }

        public MvcApplication()
        {
            BeginRequest += this.Application_BeginRequest;
            EndRequest += this.Application_EndRequest;
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

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

            Initialize();
        }

        private void Initialize() 
        {
            DocumentStoreHolder.Initialize();
            InitializeIoC();
        }

        private void InitializeIoC()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            
            builder.Register(c => CurrentSession).As<IDocumentSession>();

            builder.RegisterType<EventoRepository>().As<Eventos>();
            builder.RegisterType<GrupoRepository>().As<Grupos>();

            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<EventoCanceladoEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<ReservaDeLocalCanceladaEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<HorarioDeReservaDeLocalAlteradoEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<HorarioDeEventoAlteradoEvent>>();
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void Application_BeginRequest(object sender, EventArgs e) 
        {
            CurrentSession = DocumentStoreHolder.DocumentStore.OpenSession();
        }

        protected void Application_EndRequest(object sender, EventArgs e) 
        {
            var context = ((MvcApplication)sender).Context;
            if (context.Error == null)
                CurrentSession.SaveChanges();

            CurrentSession.Dispose();
        }
    }
}