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
using Integer.Infrastructure.Tasks;
using Integer.Infrastructure.Repository;
using Autofac;

namespace Integer.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.Headers["Origin"] != null 
                && (Request.Headers["Origin"].Contains(".calendarioparoquial.com.br")
                    || Request.Headers["Origin"].Contains("localhost"))) 
                Response.AppendHeader("Access-Control-Allow-Origin", Request.Headers["Origin"]);
        }
    }
}
