using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.DTO;
using Web.Helpers;

namespace Web.Controllers
{
    public class AcessoController : Controller
    {
        protected WebApiHelper WebApi 
        {
            get 
            {
                var domain = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.AbsolutePath));
                return new WebApiHelper(new Uri(new Uri(domain), "/api/Usuario"));
            }
        }

        public JsonResult Login(string email, string senha, bool? lembrar)
        {
            var usuario = WebApi.Get<Usuario>("email="+email, "senha="+senha);
            return Json(usuario);
        }
    }
}
