using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Integer.Domain.Paroquia;
using System.Dynamic;
using System.Web.Security;
using Integer.Web.Infra.Raven;
using System.Net;
using Integer.Web.ViewModels;
using Integer.Web.Infra.AutoMapper;

namespace Integer.Web.Controllers
{
    public class UsuarioController : ControllerBase
    {
        private readonly Grupos grupos;

        public UsuarioController(Grupos grupos)
        {
            this.grupos = grupos;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string senha, bool? lembrar)
        {
            Grupo grupo = grupos.Com(g => g.Email == email);
            if (grupo != null && grupo.PrecisaCriarUsuario)
            {
                if (grupo.ValidarSenha(senha))
                {
                    TempData["GrupoId"] = grupo.Id;
                    TempData["GrupoEmail"] = grupo.Email;
                    return RedirectToAction("Criar");
                }
                else 
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else if (RavenSession.ValidaUsuario(email, senha))
            {
                FormsAuthentication.SetAuthCookie(email, lembrar.GetValueOrDefault());

                var grupoIdCrypt = FormsAuthentication.HashPasswordForStoringInConfigFile(grupo.Id, "MD5");
                var grupoIdCookie = new HttpCookie("gid", grupoIdCrypt);
                grupoIdCookie.Expires = DateTime.MaxValue;
                grupoIdCookie.HttpOnly = false;
                Response.Cookies.Add(grupoIdCookie);
            }
            else 
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            return null;
        }

        [HttpGet]
        public ActionResult Criar() 
        {
            string grupoId = "", grupoEmail = "";
            if (TempData["GrupoId"] != null && TempData["GrupoEmail"] != null)
            {
                grupoId = TempData["GrupoId"].ToString();
                grupoEmail = TempData["GrupoEmail"].ToString();
            }
            return View(new UsuarioCriarViewModel { GrupoId = grupoId, Email = grupoEmail });
        }

        [HttpPost]
        public ActionResult Criar(UsuarioCriarViewModel usuarioInput) 
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("CriarForm", usuarioInput);
            }
            try
            {
                var usuario = usuarioInput.MapTo<Usuario>();
                var grupo = RavenSession.Query<Grupo>().FirstOrDefault(g => g.Email == usuarioInput.Email);
                
                RavenSession.CriarUsuario(usuario, grupo);
                FormsAuthentication.SetAuthCookie(usuarioInput.Email, false);
            }
            catch (UsuarioExistenteException ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { ErrorMessage = ex.Message });
            }            
            return null;
        }

        [HttpGet]
        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult Menu() 
        {
            // TODO: get menu for current user
            return PartialView("Menu");
        }
    }
}
