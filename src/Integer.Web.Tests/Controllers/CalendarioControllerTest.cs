using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Web.Controllers;
using MvcContrib.TestHelper;

namespace Integer.Web.Tests.Controllers
{
    [TestFixture]
    public class CalendarioControllerTest
    {
        [Test]
        public void ActionPadrao_DeveRetornarViewCalendario() 
        {
            var calendarioController = new CalendarioController(null, null);

            var result = calendarioController.Index();

            result.AssertViewRendered().ForView("Calendario");
        }
    }
}
