using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Integer.Web.Controllers
{
    public class ParoquiaController : Controller
    {
        [HttpGet]
        public JsonResult Locals()
        {
            // TODO get from database
            return Json(new List<object> { new { Id = 1, Name = "Templo" }, new { Id = 2, Name = "Salão A" } }, JsonRequestBehavior.AllowGet);
        }
    }
}
