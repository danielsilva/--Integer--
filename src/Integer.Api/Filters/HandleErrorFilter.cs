using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using Elmah;
using Integer.Api.Controllers;

namespace Integer.Api.Filters
{
    public class HandleErrorFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);

            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as BaseController;
            if (controller != null)
                controller.DoNotCallSaveChanges = true;

            ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);
            
        }
    }
}