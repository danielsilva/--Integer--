using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Integer.Api.Infra.AutoMapper;
using Integer.Domain.Acesso;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Events;
using Integer.Infrastructure.IoC;
using Integer.Infrastructure.Repository;

namespace Integer.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            Initialize(config);
        }

        private static void Initialize(HttpConfiguration config)
        {
            DocumentStoreHolder.Initialize();

            InitializeIoC(config);
            AutoMapperConfiguration.Configure();
        }

        private static void InitializeIoC(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            //builder.ConfigureWebApi(GlobalConfiguration.Configuration);

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

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
            config.DependencyResolver = resolver;

            IoCWorker.Initialize(resolver);
        }
    }
}
