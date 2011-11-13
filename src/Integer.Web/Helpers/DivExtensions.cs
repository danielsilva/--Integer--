using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Integer.Web.Helpers {
    public static class DivExtensions 
    {
        public static void BeginDiv(this HtmlHelper htmlHelper, object htmlAttributes) 
        { 
            TagBuilder builder = new TagBuilder("div");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            htmlHelper.ViewContext.HttpContext.Response.Write(builder.ToString(TagRenderMode.StartTag));
        }

        public static void EndDiv(this HtmlHelper htmlHelper) 
        {
            htmlHelper.ViewContext.HttpContext.Response.Write("</div>");
        }
    }
}
