using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundle/jmCustomScrollBar").Include(
                "~/Scripts/Plugins/jQueryScrollBar/jquery.mousewheel.js",
                "~/Scripts/Plugins/jQueryScrollBar/jquery.mCustomScrollbar.js"));

            bundles.Add(new ScriptBundle("~/bundle/jUITimePicker").Include(
                "~/Scripts/Plugins/jQueryUITimePicker/jquery-ui-sliderAccess.js",
                "~/Scripts/Plugins/jQueryUITimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/Plugins/jQueryUITimePicker/jquery-ui-timepicker-pt-BR.js"));

            bundles.Add(new ScriptBundle("~/bundle/site").Include(
                "~/Scripts/Shared/jquery.validation.js",
                "~/Scripts/Usuario/login.js",
                "~/Scripts/Plugins/jQueryCookie/jquery.cookies.2.2.0.js",
                "~/Scripts/Plugins/alerts/jquery.alerts.min.js",
                "~/Scripts/shared/global.js",
                "~/Scripts/shared/date.js"));

            bundles.Add(new ScriptBundle("~/bundle/offSite").Include(
                "~/Scripts/Shared/jquery.validation.js",
                "~/Scripts/Shared/global.js"));

            bundles.Add(new ScriptBundle("~/bundle/site/calendario").Include(
                "~/Scripts/Plugins/extCalendar/extensible-core.js",
                "~/Scripts/Plugins/extCalendar/recurrence-debug.js",
                "~/Scripts/Plugins/extCalendar/calendar-debug.js",
                "~/Scripts/Plugins/extCalendar/locale/ext-lang-pt_BR.js",
                "~/Scripts/Calendario/calendario.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css/site").Include(
                "~/Content/Site.css",
                "~/Content/Validation.css",
                "~/Content/alerts/jquery.alerts.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/offSite").Include(
                "~/Content/OffSite.css",
                "~/Content/Validation.css"));

            bundles.Add(new StyleBundle("~/Content/css/jqueryUI").Include(
                "~/Content/themes/base/jquery-ui-redmond.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.slider.css"));

            bundles.Add(new StyleBundle("~/Content/css/jqueryPlugins").Include(
                "~/Content/jQueryScrollBar/jquery.mCustomScrollbar.css",
                "~/Content/jQueryUITimePicker/jquery-ui-timepicker-addon.css"));

            bundles.Add(new StyleBundle("~/Content/css/calendario").Include(
                "~/Content/extCalendar/css/extensible-all.css",
                "~/Content/calendario.css"));
        }
    }
}