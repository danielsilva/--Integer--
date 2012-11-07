using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Integer.Domain.Paroquia;
using Integer.Web.Infra.AutoMapper;
using Integer.Web.ViewModels;
using Integer.Web.Infra.Raven;

namespace Integer.Web.Controllers
{
    public class ParoquiaController : ControllerBase
    {
        [HttpGet]
        public JsonResult Locais()
        {
            var locais = RavenSession.ObterLocais();
            return Json(locais.MapTo<ItemViewModel>(), JsonRequestBehavior.AllowGet);
        }
    }
}
