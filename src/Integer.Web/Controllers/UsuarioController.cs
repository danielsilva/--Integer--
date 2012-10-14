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
        public void Login(string email, string senha, bool? lembrar)
        {
            Grupo grupo = grupos.Com(g => g.Email == email);
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
                    // TODO: redirect to Trocar Senha
                    //return PartialView("TrocaSenha", usuarioDinamico);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(email, lembrar.GetValueOrDefault());
                    Response.AddHeader("Location", "/Calendario");
                }
            }
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
