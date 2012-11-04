using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elmah;

namespace Integer.Web.Infra.Elmah
{
    public class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            if (!filterContext.ExceptionHandled)
                return;

            var httpContext = filterContext.HttpContext.ApplicationInstance.Context;
            var signal = ErrorSignal.FromContext(httpContext);
            signal.Raise(filterContext.Exception, httpContext);
        }
    }
}