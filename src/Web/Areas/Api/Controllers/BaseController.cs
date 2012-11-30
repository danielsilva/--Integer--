using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Integer.Domain.Paroquia;
using Raven.Client;
using Integer.Api.Infra.Raven;
using Integer.Domain.Acesso;
using System.Web.Http.Controllers;
using System.Threading;
using Integer.Infrastructure.Repository;
using System.Threading.Tasks;
using Integer.Infrastructure.IoC;
using Web;
using Web.Areas.Api.Security;

namespace Web.Areas.Api.Controllers
{
    public class BaseController : ApiController
    {
        protected Usuario UsuarioLogado 
        {
            get 
            {
                if (!HttpContext.Current.Request.IsAuthenticated)
                    return null;

                return ((ApiIdentity)HttpContext.Current.User).Usuario;
            }
        }

        protected Grupo GrupoLogado
        {
            get
            {
                if (UsuarioLogado == null || String.IsNullOrWhiteSpace(UsuarioLogado.GrupoId))
                    return null;

                return RavenSession.ObterGrupoPorId(UsuarioLogado.GrupoId);
            }
        }

        public IDocumentSession RavenSession { get; set; }

        public bool DoNotCallSaveChanges { get; set; }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            RavenSession = WebApiConfig.CurrentSession;

            return base.ExecuteAsync(controllerContext, cancellationToken)
                .ContinueWith(task => 
                {
                    using (RavenSession) 
                    {
                        if (task.Status != TaskStatus.Faulted && RavenSession != null && DoNotCallSaveChanges == false)
                            RavenSession.SaveChanges();
                    }
                    return task;
                }).Unwrap();
        }
    }
}
