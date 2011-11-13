using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using System.Linq.Expressions;

namespace Integer.Web.Helpers {
    
    public static class ValidationExtensions {

        public static string ValidationImage(this HtmlHelper htmlHelper, string modelName) 
        {
            return htmlHelper.ValidationImage(modelName, null);
        }

        public static string ValidationImage(this HtmlHelper htmlHelper, string modelName, object htmlAttributes) 
        {
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(modelName);
            if (!htmlHelper.ViewData.ModelState.ContainsKey(fullHtmlFieldName))
            {
                return null;
            }
            ModelState modelState = htmlHelper.ViewData.ModelState[fullHtmlFieldName];
            ModelErrorCollection errors = (modelState == null) ? null : modelState.Errors;
            ModelError error = ((errors == null) || (errors.Count == 0)) ? null : errors[0];
            if ((error == null))
            {
                return null;
            }

            string idValidationImage = fullHtmlFieldName.Replace(".", "_").Replace("[", "").Replace("]", "") + "_ValidationImage";
            string idDivMensagensErro = "div" + idValidationImage;

            StringBuilder divMensagensErro = new StringBuilder("<div id='" + idDivMensagensErro + "' style=\"display:none;\">");
            foreach (ModelState state in htmlHelper.ViewData.ModelState.Values)
            {
                if (state == modelState)
                {
                    foreach (ModelError erro in state.Errors)
                    {
                        divMensagensErro.Append("<p>- " + erro.ErrorMessage + "</p>");
                    }
                }
            }
            divMensagensErro.Append("</div>");
            htmlHelper.ViewContext.HttpContext.Response.Write(divMensagensErro.ToString());

            StringBuilder javaScript = new StringBuilder();
            javaScript.Append("<script type='text/javascript'>");
            javaScript.Append("  $('#" + idValidationImage + "').bt({ contentSelector: \"$('#" + idDivMensagensErro + "').html()\", fill: 'rgba(255, 247, 191, .9)', strokeWidth: 2, strokeStyle: '#E86857' });");
            javaScript.Append("</script>");
            htmlHelper.ViewContext.HttpContext.Response.Write(javaScript.ToString());

            TagBuilder builder = new TagBuilder("img");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            builder.MergeAttribute("id", idValidationImage);
            builder.MergeAttribute("src", "/integer2/Content/images/exclamacao.png");
            builder.MergeAttribute("class", "jTip");

            return builder.ToString(TagRenderMode.SelfClosing);
        }        

        public static string ValidationImageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) 
        {
            return htmlHelper.ValidationImageFor(expression, null);
        }

        public static string ValidationImageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return htmlHelper.ValidationImageFor(ExpressionHelper.GetExpressionText((LambdaExpression)expression), null, htmlAttributes);
        }

        public static string ValidationImageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string uniqueFieldKey)
        {
            return htmlHelper.ValidationImageFor(ExpressionHelper.GetExpressionText((LambdaExpression)expression), uniqueFieldKey, null);
        }

        public static string ValidationImageFor<TModel>(this HtmlHelper<TModel> htmlHelper, string expression, string uniqueFieldKey, object htmlAttributes)
        {
            string fieldName = uniqueFieldKey ?? expression;

            return htmlHelper.ValidationImage(fieldName, htmlAttributes);
        }

        public static string JQueryValidationTitle(this HtmlHelper htmlHelper, string message) {
            return htmlHelper.JQueryValidationTitle(message, null);
        }

        public static string JQueryValidationTitle(this HtmlHelper htmlHelper, string message, object htmlAttributes)
        {
            string str;
            if (htmlHelper.ViewData.ModelState.IsValid) {
                return null;
            }

            if (!string.IsNullOrEmpty(message)) {
                TagBuilder builder = new TagBuilder("div");
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                builder.MergeAttribute("class", "ui-state-error ui-corner-all");
                builder.MergeAttribute("style", "padding: 0 .7em;");

                builder.InnerHtml = "<p><span class=\"ui-icon ui-icon-alert\" style=\"float: left; margin-right: .3em;\"></span><strong>Atenção:&nbsp;</strong>" 
                                    + message 
                                    + "</p></div>";
                str = builder.ToString(TagRenderMode.Normal);
            }
            else {
                str = null;
            }

            return str;
        }
    }
}
