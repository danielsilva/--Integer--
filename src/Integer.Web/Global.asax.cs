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
using Raven.Database.Server;
using Integer.Web.Infra.AutoMapper;
using Integer.Infrastructure.Email;
using System.Text;
using Integer.Infrastructure.IoC;
using Integer.Domain.Acesso;
using Integer.Infrastructure.Tasks;

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
            AutoMapperConfiguration.Configure();
        }

        private void InitializeIoC()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            
            builder.Register(c => CurrentSession).As<IDocumentSession>();

            builder.RegisterType<EventoRepository>().As<Eventos>();
            builder.RegisterType<GrupoRepository>().As<Grupos>();
            builder.RegisterType<LocalRepository>().As<Locais>();
            builder.RegisterType<UsuarioRepository>().As<Usuarios>();
            builder.RegisterType<UsuarioTokenRepository>().As<UsuarioTokens>();

            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<EventoCanceladoEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<ReservaDeLocalCanceladaEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<HorarioDeReservaDeLocalAlteradoEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<HorarioDeEventoAlteradoEvent>>();

            builder.Register<AgendaEventoService>(c => new AgendaEventoService(c.Resolve<Eventos>()));
            builder.Register<TrocaSenhaService>(c => new TrocaSenhaService(c.Resolve<Usuarios>(), c.Resolve<UsuarioTokens>()));

            var container = builder.Build();
            var resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
            IoCWorker.Initialize(resolver);
        }

        protected void Application_BeginRequest(object sender, EventArgs e) 
        {
            CurrentSession = DocumentStoreHolder.DocumentStore.OpenSession();
        }

        protected void Application_EndRequest(object sender, EventArgs e) 
        {
            using (var session = CurrentSession)
            {
                if (session == null)
                    return;

                if (Server.GetLastError() != null)
                    return;

                session.SaveChanges();
            }
            TaskExecutor.StartExecuting();
        }
    }
}