using System.Web;
using System.Web.Optimization;

namespace StudyDoIT
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js",
                       "~/Scripts/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery1").Include(
                       "~/Scripts/jsadmin/modernizr.js",
                       "~/Scripts/jsadmin/jquery-2.1.1.min.js",
                       "~/Scripts/bootstrap.min.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/ajaxform").Include(
                                       "~/Scripts/jquery.unobtrusive-ajax*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));
            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Content/bs3/js/bootstrap.min.js",
                      "~/Scripts/js/bootstrap-datepicker.js",
                      "~/Scripts/js/jquery.minimalect.min.js",
                      "~/Scripts/js/jquery.flexslider-min.js",
                      "~/Scripts/js/rs-plugin/js/jquery.plugins.min.js",
                      "~/Scripts/js/rs-plugin/js/jquery.revolution.min.js",
                      "~/Scripts/js/script.js"));

            //bundles.Add(new ScriptBundle("~/bundles/news").Include(
            //          "~/Scripts/custom.js",
            //          "~/Scripts/flex-slider-min.js",
            //          "~/Scripts/plugins.js"));

            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                            "~/Scripts/jsadmin/jquery.slimscroll.min.js",
                            "~/Scripts/jsadmin/jquery.placeholder.js",
                            "~/Scripts/jsadmin/offscreen.js",
                            "~/Scripts/jsadmin/main.js",
                            "~/Scripts/jsadmin/jszip.min.js",
                            "~/Scripts/jsadmin/kendo.all.min.js",
                            "~/Scripts/jsadmin/jquery.unobtrusive-ajax.min.js",
                            "~/Scripts/jsadmin/kendo.aspnetmvc.min.js"
                ));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/css/layout.css",
            //          "~/Content/css/skeleton.css",
            //          "~/Content/css/style.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(

                      "~/Content/css/bs3/css/bootstrap.css",
                      "~/Content/bootstrap.css",
                      "~/Content/css/styles.css",
                      "~/Content/css/flexslider.css",
                      "~/Content/css/responsive.css",
                      "~/Content/css/colors/color1.css",
                      "~/Content/css/flexslider.css",
                      "~/Content/css/responsive.css"));
        }
    }
}
