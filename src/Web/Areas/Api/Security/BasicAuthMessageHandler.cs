using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using Integer.Domain.Acesso;
using Integer.Domain.Acesso.Services;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.IoC;

namespace Web.Areas.Api.Security
{
    public class BasicAuthMessageHandler : DelegatingHandler
    {
        private readonly AutenticaUsuarioService autenticaUsuarioService;

        public BasicAuthMessageHandler()
        {
            this.autenticaUsuarioService = new AutenticaUsuarioService(IoCWorker.Resolve<Grupos>(), IoCWorker.Resolve<Usuarios>());
        }
        
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue authValue = request.Headers.Authorization;
            if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter))
            {
                Credentials parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);
                if (parsedCredentials != null)
                {
                    Usuario usuario = autenticaUsuarioService.Autenticar(parsedCredentials.Username, parsedCredentials.Password);
                    if (usuario != null)
                        Thread.CurrentPrincipal = new GenericPrincipal(new ApiIdentity(usuario), new string[] { });
                }
            }
            return base.SendAsync(request, cancellationToken);
        }

        private Credentials ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert
                                                            .FromBase64String(authHeader))
                                                            .Split(
                                                            new[] { ':' });
            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
                return null;

            return new Credentials()
            {
                Username = credentials[0],
                Password = credentials[1],
            };
        }       
    }
}