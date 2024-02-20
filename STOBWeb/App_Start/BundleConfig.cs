using System.Web;
using System.Web.Optimization;

namespace STOBWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/Library/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Bootstrap/bootstrap.js",
                      "~/Scripts/Library/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Bootstrap/bootstrap.css",
                      "~/Content/jquery/jquery-ui.css",
                      "~/Content/Bootstrap/bootstrap-vertical-tabs.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesomeCss").Include(
                "~/Content/font-awesome.min.css",
                "~/fonts/fontawesome-webfont.eot",
                "~/fonts/fontawesome-webfont.svg",
                "~/fonts/fontawesome-webfont.ttf",
                "~/fonts/fontawesome-webfont.woff",
                "~/fonts/fontawesome-webfont.woff2",
                "~/fonts/fontawesome.otf"));

            bundles.Add(new ScriptBundle("~/bundles/parsley").Include(
                        "~/Scripts/Parsley/parsley.min.js"));
        }
    }
}
