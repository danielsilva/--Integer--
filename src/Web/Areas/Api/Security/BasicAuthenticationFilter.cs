using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using Integer.Domain.Acesso;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Criptografia;
using Integer.Infrastructure.IoC;

namespace Integer.Api.Security
{
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        private readonly Grupos grupos;
        private readonly Usuarios usuarios;

        public BasicAuthenticationAttribute()
        {
            this.grupos = IoCWorker.Resolve<Grupos>();
            this.usuarios = IoCWorker.Resolve<Usuarios>();
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
 
                string email = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                string senha = decodedToken.Substring(decodedToken.IndexOf(":") + 1);
                
                Usuario usuario = this.ObterUsuario(email, senha);
                if (usuario != null)
                {
                    HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(usuario), new string[] { }); // TODO: user roles
                    base.OnActionExecuting(actionContext);
                }
                else
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var usuarioPrecisaTrocarSenha = ((ApiIdentity)HttpContext.Current.User.Identity).Usuario.PrecisaTrocarSenha;
            if (usuarioPrecisaTrocarSenha) 
            {
                actionExecutedContext.Response.StatusCode = HttpStatusCode.RedirectMethod;
                HttpContext.Current.Response.Headers.Set("Location", actionExecutedContext.Request.RequestUri.Host + "/Usuario/Criar");
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        private Usuario ObterUsuario(string email, string senha) 
        {
            Usuario usuario = null;

            Grupo grupo = grupos.Com(g => g.Email == email);
            if (grupo != null && grupo.PrecisaCriarUsuario)
            {
                if (grupo.ValidarSenha(senha))
                {
                    usuario = new Usuario(email, senha, grupo.Id);
                    usuarios.Salvar(usuario);
                }
            }
            else 
            {
                usuario = usuarios.Com(u => u.Email == email && u.Senha == Encryptor.Encrypt(senha));
            }
            return usuario;
        }
    }
}