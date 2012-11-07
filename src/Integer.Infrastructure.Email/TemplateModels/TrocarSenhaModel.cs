using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Integer.Infrastructure.Criptografia;

namespace Integer.Infrastructure.Email.TemplateModels
{
    public class TrocarSenhaModel
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Link
        {
            get
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var url = urlHelper.Action("TrocarSenha", "Usuario");
                var queryString = String.Format("?id={0}&token={1}", urlHelper.Encode(Encryptor.Encrypt(UserId)), Token);

                return url + queryString;
            }
        }
    }
}
