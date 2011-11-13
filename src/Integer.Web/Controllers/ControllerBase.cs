using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Integer.Infrastructure.IoC;
using DbC;
using Integer.Domain.Paroquia;

namespace Integer.Web.Controllers
{
    [HandleError]
    public class ControllerBase : Controller
    {
        protected bool UsuarioEstaLogado 
        {
            get 
            {
                return Request.IsAuthenticated;
            }
        }

        private Grupo grupo;
        protected Grupo GrupoLogado
        {
            get 
            {
                if (UsuarioEstaLogado) 
                {
                    grupo = DependencyResolver.Current.GetService<Grupos>().Com(g => g.Email == HttpContext.User.Identity.Name);
                }
                return grupo;
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (!ModelState.IsValid)
            {
                filterContext.HttpContext.Response.StatusCode = 201;
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            if (filterContext.Exception is DbCException)
            {
                HandleError(filterContext.Exception);
                filterContext.ExceptionHandled = true;
                return;
            }
            string login = " sem login";
            if (User != null)
            {
                login = User.Identity.Name;
            }

            // TODO log de erros
            //ILog log = LogManager.GetLogger("EmailLogger");
            //log.Error("Erro na Aplicação " + login, Server.GetLastError());

            //ILog filelog = LogManager.GetLogger("FileLogger");
            //filelog.Error("Erro na Aplicação", Server.GetLastError());
        }

        protected void HandleError(Exception ex) 
        {
            string msg = ex.Message.Replace("\r\n", "\n");
            Response.AddHeader("ERRO", HttpUtility.HtmlEncode(msg));
        }
    }
}
