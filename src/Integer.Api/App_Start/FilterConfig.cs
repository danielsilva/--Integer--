using System.Web;
using System.Web.Mvc;
using Integer.Api.Filters;

namespace Integer.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorFilter());
            filters.Add(new ValidateModelFilter());
        }
    }
}