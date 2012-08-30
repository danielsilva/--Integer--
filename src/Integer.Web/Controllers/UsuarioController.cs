using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Integer.Domain.Paroquia;
using System.Dynamic;
using System.Web.Security;

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
        public ActionResult Login(string login, string senha)
        {
            if (String.IsNullOrEmpty(login))
                ModelState.AddModelError("login", "Preenchimento obrigatório.");

            if (String.IsNullOrEmpty(senha))
                ModelState.AddModelError("senha", "Preenchimento obrigatório.");

            if (ModelState.IsValid)
            {
                Grupo grupo = grupos.Com(g => g.Email == login);
                if (grupo == null || !grupo.ValidarSenha(senha))
                {
                    ModelState.AddModelError("formAcesso", "Senha inválida ou grupo não cadastrado.");
                }
                else
                {
                    if (grupo.PrecisaTrocarSenha)
                    {
                        dynamic usuarioDinamico = new ExpandoObject();
                        usuarioDinamico.login = grupo.Email;
                        return PartialView("TrocaSenha", usuarioDinamico);
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(login, false);
                        Response.AddHeader("Location", "/Calendario");
                    }
                }
            }
            return PartialView("LoginForm");
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

        [HttpPost]
        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            Response.AddHeader("Location", "/");
            return View("Login");
        }
    }
}
