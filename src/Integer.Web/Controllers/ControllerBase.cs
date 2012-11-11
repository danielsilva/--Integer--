using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Integer.Infrastructure.IoC;
using DbC;
using Integer.Domain.Paroquia;
using Raven.Client;
using Integer.Web.Infra.Raven;
using IntegerElmah = Integer.Web.Infra.Elmah;
using Integer.Infrastructure.Tasks;
using Integer.Infrastructure.Repository;
using Integer.Infrastructure.Transaction;
using Autofac;

namespace Integer.Web.Controllers
{
    [IntegerElmah.HandleError]
    public class ControllerBase : Controller
    {
        protected bool UsuarioEstaLogado 
        {
            get 
            {
                return Request.IsAuthenticated;
            }
        }

        protected Grupo GrupoLogado 
        {
            get 
            {
                if (!HttpContext.Request.IsAuthenticated)
				    return null;

			    var email = HttpContext.User.Identity.Name;
                return RavenSession.ObterGrupoPorEmail(email);
            }
        } 

        public IDocumentSession RavenSession { get; set; }

        protected bool DoNotCallSaveChanges 
        {
            set 
            {
                MvcApplication.RequestCannotSaveChanges = value;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RavenSession = MvcApplication.CurrentSession;

            if (Request.Headers["Origin"] != null 
                && (Request.Headers["Origin"].Contains(".calendarioparoquial.com.br")
                    || Request.Headers["Origin"].Contains("localhost"))) 
                Response.AppendHeader("Access-Control-Allow-Origin", Request.Headers["Origin"]);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            DoNotCallSaveChanges = true;

            base.OnException(filterContext);
        }
    }
}
