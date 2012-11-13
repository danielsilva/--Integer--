using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Integer.Api.Infra.AutoMapper;
using Integer.Domain.Acesso;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Events;
using Integer.Infrastructure.IoC;
using Integer.Infrastructure.Repository;
using Raven.Client;

namespace Integer.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private const string RavenSessionKey = "RavenDB.Session";
        private const string RequestSaveChangesKey = "RequestSaveChanges";

        public static IDocumentStore DocumentStore { get; set; }

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

        public static bool RequestCannotSaveChanges
        {
            get
            {
                return (HttpContext.Current.Items[RequestSaveChangesKey] as bool?) ?? false;
            }
            set
            {
                HttpContext.Current.Items[RequestSaveChangesKey] = value;
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Initialize();
        }

        private void Initialize()
        {
            DocumentStoreHolder.Initialize();
            DocumentStore = DocumentStoreHolder.DocumentStore;

            InitializeIoC();
            AutoMapperConfiguration.Configure();
        }

        private void InitializeIoC()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(WebApiApplication).Assembly);

            builder.Register(c => CurrentSession).As<IDocumentSession>();

            builder.RegisterType<EventoRepository>().As<Eventos>();
            builder.RegisterType<GrupoRepository>().As<Grupos>();
            builder.RegisterType<LocalRepository>().As<Locais>();
            builder.RegisterType<UsuarioRepository>().As<Usuarios>();
            builder.RegisterType<UsuarioTokenRepository>().As<UsuarioTokens>();

            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<EventoCanceladoEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<ReservaDeLocalAlteradaEvent>>();
            builder.RegisterType<RemoveConflitoService>().As<DomainEventHandler<HorarioDeEventoAlteradoEvent>>();

            builder.Register<AgendaEventoService>(c => new AgendaEventoService(c.Resolve<Eventos>()));
            builder.Register<TrocaSenhaService>(c => new TrocaSenhaService(c.Resolve<Usuarios>(), c.Resolve<UsuarioTokens>()));

            var container = builder.Build();
            var resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
            IoCWorker.Initialize(resolver);
        }
    }
}