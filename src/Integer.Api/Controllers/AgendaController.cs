using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Integer.Api.Controllers
{
    public class AgendaController : BaseController
    {
        public IEnumerable<dynamic> Get()
        {
            return new List<dynamic>{
                new { id = 1, title = "", desc = "", color = "", hidden = "false" },
                new { id = 2, title = "", desc = "", color = "FF4FDB", hidden = "false" }
            };
        }
    }
}
