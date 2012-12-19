using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Integer.Api.Infra.AutoMapper;
using Integer.Domain.Acesso;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Events;
using Integer.Infrastructure.IoC;
using Integer.Infrastructure.Repository;
using Raven.Client;
using Autofac.Integration.WebApi;
using Web.Areas.Api.Security;

namespace Web
{
    public static class WebApiConfig
    {
        public const string RavenSessionKey = "RavenDB.Session";

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

        public static void Register(HttpConfiguration config)
        {
            InitializeDocumentStore();
            ConfigIoC();
            ConfigAutoMapper();

            ConfigWebApi(config);
        }

        private static void InitializeDocumentStore()
        {
            DocumentStoreHolder.Initialize();
        }

        private static void ConfigIoC()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(c =>
            {
                if (CurrentSession == null)
                    CurrentSession = DocumentStoreHolder.DocumentStore.OpenSession();

                return CurrentSession;
            }).As<IDocumentSession>();

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
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            IoCWorker.Initialize(resolver);
        }

        private static void ConfigAutoMapper()
        {
            AutoMapperConfiguration.Configure();
        }

        private static void ConfigWebApi(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new HttpsHandler());
            config.MessageHandlers.Add(new BasicAuthMessageHandler());
        }
    }
}
