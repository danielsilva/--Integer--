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
            if (grupo != null && grupo.ValidarSenha(senha))
            {
                if (grupo.PrecisaCriarUsuario)
                {
                    TempData["GrupoId"] = grupo.Id;
                    return RedirectToAction("Criar");
                }
                else
                {
                    if (RavenSession.ValidaUsuario(email, senha))
                    {
                        FormsAuthentication.SetAuthCookie(email, lembrar.GetValueOrDefault());
                    }
                }
            }
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return null;
        }

        [HttpGet]
        public ActionResult Criar() 
        {
            string grupoId = "";
            if (TempData["GrupoId"] != null)
                grupoId = TempData["GrupoId"].ToString();

            return View(new UsuarioCriarViewModel { GrupoId = grupoId });
        }

        [HttpPost]
        public ActionResult Criar(UsuarioCriarViewModel usuarioInput) 
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("CriarForm", usuarioInput);
            }

            //RavenSession.Store(usuarioInput.MapTo(usuario));
            return null;
        }

        [HttpPost]
        public ActionResult TrocarSenha(string login, string novaSenha, string novaSenhaRepetida)
        {
            if (String.IsNullOrEmpty(novaSenha))
                ModelState.AddModelError("novaSenha", "Preenchimento obrigatório.");

            if (String.IsNullOrEmpty(novaSenhaRepetida))
                ModelState.AddModelError("novaSenhaRepetida", "Preenchimento obrigatório.");

            if (ModelState.IsValid)
            {
                if (!novaSenha.Equals(novaSenhaRepetida))
                {
                    ModelState.AddModelError("formSenha", "Os conteúdos informados precisam ser iguais.");
                }
                else
                {
                    Grupo grupo = grupos.Com(g => g.Email == login);
                    if (grupo != null)
                    {
                        grupo.TrocarSenha(novaSenha);
                        FormsAuthentication.SetAuthCookie(grupo.Email, false);
                        Response.AddHeader("Location", "/Calendario");
                    }
                }
            }
            dynamic usuarioDinamico = new ExpandoObject();
            usuarioDinamico.login = login;
            return PartialView("TrocaSenha", usuarioDinamico);
        }

        [HttpGet]
        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}
