using System.Web;
using System.Web.Optimization;

namespace BladderChange.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //app.js
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app.js"));

            //app-ext.js
            bundles.Add(new ScriptBundle("~/bundles/app-ext").Include(
                "~/Scripts/app-ext.js"));

            //DataTables
            //bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
            //    "~/Scripts/DataTables/jquery.dataTables.js"));

            //bundles.Add(new StyleBundle("~/Content/datatables").Include(
            //    "~/Content/DataTables/css/jquery.dataTables.css"));
        }
    }
}
