using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Integer.Web.ViewModels.MailTemplates
{
    public class LembrarSenhaViewModel
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Link 
        { 
            get 
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var url = urlHelper.Action("TrocarSenha", "Usuario");
                var queryString = String.Format("?id={0}&token={1}", UserId, Token);

                return url + queryString;
            } 
        }
    }
}