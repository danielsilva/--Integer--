using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Integer.Domain.Acesso;

namespace Web.Areas.Api.Security
{
    public class ApiIdentity : IIdentity
    {
        public Usuario Usuario { get; private set; }

        public ApiIdentity(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException("usuario");

            this.Usuario = usuario;
        }

        public string Name
        {
            get { return this.Usuario.Email; }
        }

        public string AuthenticationType
        {
            get { return "Basic"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }
    }
}