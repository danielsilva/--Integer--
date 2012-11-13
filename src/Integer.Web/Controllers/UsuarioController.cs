//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Integer.Domain.Paroquia;
//using System.Dynamic;
//using System.Web.Security;
//using Integer.Web.Infra.Raven;
//using System.Net;
//using Integer.Web.ViewModels;
//using Integer.Web.Infra.AutoMapper;
//using Integer.Domain.Acesso;
//using Integer.Domain.Acesso.Exceptions;
//using Integer.Infrastructure.Criptografia;

//namespace Integer.Web.Controllers
//{
//    public class UsuarioController : ControllerBase
//    {
//        private readonly Grupos grupos;
//        private readonly TrocaSenhaService trocaSenhaService;

//        public UsuarioController(Grupos grupos, TrocaSenhaService trocaSenhaService)
//        {
//            this.grupos = grupos;
//            this.trocaSenhaService = trocaSenhaService;
//        }

//        //[HttpGet]
//        //public ActionResult Login()
//        //{
//        //    return View();
//        //}

//        //[HttpPost]
//        //public ActionResult Login(string email, string senha, bool? lembrar)
//        //{
//        //    Grupo grupo = grupos.Com(g => g.Email == email);
//        //    if (grupo != null && grupo.PrecisaCriarUsuario)
//        //    {
//        //        if (grupo.ValidarSenha(senha))
//        //        {
//        //            TempData["GrupoId"] = grupo.Id;
//        //            TempData["GrupoEmail"] = grupo.Email;
//        //            return RedirectToAction("Criar");
//        //        }
//        //        else 
//        //        {
//        //            HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //            return null;
//        //        }
//        //    }
//        //    else if (RavenSession.ValidaUsuario(email, senha))
//        //    {
//        //        FormsAuthentication.SetAuthCookie(email, lembrar.GetValueOrDefault());

//        //        var grupoIdCrypt = FormsAuthentication.HashPasswordForStoringInConfigFile(grupo.Id, "MD5");
//        //        var grupoIdCookie = new HttpCookie("gid", grupoIdCrypt);
//        //        grupoIdCookie.Expires = DateTime.MaxValue;
//        //        grupoIdCookie.HttpOnly = false;
//        //        Response.Cookies.Add(grupoIdCookie);
//        //    }
//        //    else 
//        //    {
//        //        HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //        return null;
//        //    }
//        //    return Redirect("/");
//        //}

//        //[HttpGet]
//        //public ActionResult Criar() 
//        //{
//        //    string grupoId = "", grupoEmail = "";
//        //    if (TempData["GrupoId"] != null && TempData["GrupoEmail"] != null)
//        //    {
//        //        grupoId = TempData["GrupoId"].ToString();
//        //        grupoEmail = TempData["GrupoEmail"].ToString();
//        //    }
//        //    return View(new UsuarioCriarViewModel { GrupoId = grupoId, Email = grupoEmail });
//        //}

//        //[HttpPost]
//        //public ActionResult Criar(UsuarioCriarViewModel usuarioInput) 
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.PartialContent;
//        //        return PartialView("CriarForm", usuarioInput);
//        //    }
//        //    try
//        //    {
//        //        var usuario = usuarioInput.MapTo<Usuario>();
//        //        var grupo = RavenSession.Query<Grupo>().FirstOrDefault(g => g.Email == usuarioInput.Email);
                
//        //        RavenSession.CriarUsuario(usuario, grupo);
//        //        FormsAuthentication.SetAuthCookie(usuarioInput.Email, false);
//        //    }
//        //    catch (UsuarioExistenteException ex)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //        return Json(new { ErrorMessage = ex.Message });
//        //    }            
//        //    return null;
//        //}

//        //[HttpGet]
//        //public ActionResult Sair()
//        //{
//        //    FormsAuthentication.SignOut();
//        //    return Redirect("/");
//        //}

//        //[HttpGet]
//        //public ActionResult Menu() 
//        //{
//        //    // TODO: get menu for current user
//        //    return PartialView("Menu");
//        //}

//        //[HttpGet]
//        //[OutputCache(Duration = 3600)]
//        //public ActionResult EsqueceuSenha() 
//        //{
//        //    return View();
//        //}

//        //[HttpPost]
//        //public ActionResult EsqueceuSenha(UsuarioEsqueceuSenhaViewModel input) 
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.PartialContent;
//        //        return PartialView("TrocarSenhaForm", input);
//        //    }
//        //    try
//        //    {
//        //        trocaSenhaService.EnviarSenha(input.Email);
//        //    }
//        //    catch (UsuarioInexistenteException ex)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //        return Json(new { ErrorMessage = ex.Message });
//        //    }
//        //    return null;
//        //}

//        //[HttpGet]
//        //public ActionResult TrocarSenha(string id, Guid token, bool reset) 
//        //{
//        //    try
//        //    {
//        //        var userId = Encryptor.Decrypt(id);
//        //        if (!reset)
//        //        {
//        //            trocaSenhaService.DesativarToken(userId, token);
//        //        }
//        //        else if (trocaSenhaService.ValidarToken(userId, token))
//        //        {
//        //            return View();
//        //        }
//        //    }
//        //    catch (Exception ex) 
//        //    {
//        //        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
//        //    }
//        //    return Redirect("/");
//        //}

//        //[HttpPost]
//        //public ActionResult TrocarSenha(UsuarioTrocarSenhaViewModel input) 
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.PartialContent;
//        //        return PartialView("TrocarSenhaForm", input);
//        //    }
//        //    try
//        //    {
//        //        var userId = Encryptor.Decrypt(input.Id);
//        //        trocaSenhaService.TrocarSenha(input.Token, userId, input.Senha);
//        //    }
//        //    catch (UsuarioInexistenteException ex)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //        return Json(new { ErrorMessage = ex.Message });
//        //    }
//        //    catch (UsuarioTokenExpiradoException ex) 
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //        return Json(new { ErrorMessage = ex.Message });
//        //    }
//        //    return null;
//        //}
//    }
//}
