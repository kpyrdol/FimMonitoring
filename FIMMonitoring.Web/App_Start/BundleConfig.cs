using System.Web;
using System.Web.Optimization;

namespace FIMMonitoring.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/TwitterBootstrapMvcJs.js",
                "~/Scripts/app.js",
                "~/Scripts/Fim.js",
                "~/Scripts/bootbox.js",
                "~/Scripts/respond.js",
                "~/Scripts/plugins/pace.js"
                //"~/Scripts/plugins/icheck.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/files").Include(
                "~/Scripts/Files.js"));

            bundles.Add(new ScriptBundle("~/bundles/bswitch").Include(
                "~/Scripts/bootstrap-switch.js"));

            bundles.Add(new StyleBundle("~/Content/bswitch").Include(
                "~/Content/bootstrap-switch.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/adminLTE.css",
                "~/Content/skin-blue.css",
                "~/Content/site.css",
                "~/Content/plugins/pace.css",
                //"~/Content/plugins/icheck/blue.css",
                "~/Content/font-awesome.css"));

            #if (DEBUG)
                BundleTable.EnableOptimizations = false;
            #else
                BundleTable.EnableOptimizations = true;
            #endif

        }
    }
}
    
