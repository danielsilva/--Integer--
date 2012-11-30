using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Integer.Api.Models;
using Integer.Domain.Acesso;
using Integer.Domain.Paroquia;
using Integer.Api.Infra.AutoMapper;
using Integer.Domain.Acesso.Exceptions;
using Integer.Api.Infra.Raven;
using Integer.Infrastructure.Criptografia;

namespace Web.Areas.Api.Controllers
{
    public class UsuarioController : BaseController
    {
        private readonly Usuarios usuarios;
        private readonly Grupos grupos;
        private readonly TrocaSenhaService trocaSenhaService;

        public UsuarioController(Usuarios usuarios, Grupos grupos, TrocaSenhaService trocaSenhaService)
        {
            this.usuarios = usuarios;
            this.grupos = grupos;
            this.trocaSenhaService = trocaSenhaService;
        }
                
        [HttpPost]
        public HttpResponseMessage TrocarSenha(string id, UsuarioCriarSenhaModel usuarioInput)
        {            
            try
            {
                RavenSession.CriarSenha(id, usuarioInput.Senha);
            }
            catch (UsuarioInexistenteException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public HttpResponseMessage EsqueceuSenha(UsuarioEsqueceuSenhaModel input)
        {
            try
            {
                trocaSenhaService.EnviarSenha(input.Email);
            }
            catch (UsuarioInexistenteException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public HttpResponseMessage TrocarSenha(string id, Guid token, bool reset)
        {
            try
            {
                var userId = Encryptor.Decrypt(id);
                if (!reset)
                {
                    trocaSenhaService.DesativarToken(userId, token);
                }
                else if (!trocaSenhaService.ValidarToken(userId, token))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public HttpResponseMessage TrocarSenha(UsuarioTrocarSenhaModel input)
        {
            try
            {
                var userId = Encryptor.Decrypt(input.Id);
                trocaSenhaService.TrocarSenha(input.Token, userId, input.Senha);
            }
            catch (Exception ex)
            {
                if (ex is UsuarioInexistenteException || ex is UsuarioTokenExpiradoException)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                else
                    throw;
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public Usuario Get(string email, string senha) 
        {
            Usuario usuario = RavenSession.ObterUsuario(email, senha);
            if (usuario == null)
            {
                Grupo grupo = grupos.Com(g => g.Email == email);
                if (grupo != null && grupo.PrecisaCriarUsuario && grupo.ValidarSenha(senha))
                {
                    usuario = RavenSession.CriarUsuario(grupo.Nome, grupo.Email, grupo.Senha, grupo.Id);
                }
            }
            return usuario;
        }

        // TODO: get menu for current user
        // TODO: logout on web project
    }
}
