using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Integer.Web.Controllers
{
    public class AcessoController : Controller
    {
        [Authorize]
        public ActionResult Perfis()
        {
            return View();
        }

    }
}
