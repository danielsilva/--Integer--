using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Integer.Api.Models;
using Integer.Api.Infra.Raven;
using Integer.Api.Infra.AutoMapper;

namespace Web.Areas.Api.Controllers
{
    public class ParoquiaController : BaseController
    {
        [ActionName("Locais")]
        public IEnumerable<ItemModel> GetLocais()
        {
            var locais = RavenSession.ObterLocais();
            return locais.MapTo<ItemModel>();
        }
    }
}
