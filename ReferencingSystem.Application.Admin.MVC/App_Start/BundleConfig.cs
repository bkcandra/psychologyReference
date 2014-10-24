using System.Web;
using System.Web.Optimization;

namespace ReferencingSystem.Application.Admin.MVC
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
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminLteJs").Include(
                //Morris js
                        "~/Scripts/plugins/morris/morris.min.js",
                //Sparkline 
                        "~/Scripts/plugins/sparkline/jquery.sparkline.min.js",
                //jvectormap 
                        "~/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                        "~/Scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                //jQuery Knob Chart 
                        "~/Scripts/plugins/jqueryKnob/jquery.knob.js",
                //daterangepicker 
                        "~/Scripts/plugins/daterangepicker/daterangepicker.js",
                //datepicker 
                        "~/Scripts/plugins/datepicker/bootstrap-datepicker.js",
                //Bootstrap WYSIHTML5 
                        "~/Scripts/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                //iCheck 
                        "~/Scripts/plugins/iCheck/icheck.min.js",
                        "~/Scripts/AdminLTE/app.js"
                        ));




            bundles.Add(new StyleBundle("~/Content/adminLteCss").Include(
                      "~/Content/bootstrap.css",
                //Morris chart
                      "~/Content/css/morris/morris.css",
                //jvectormap
                      "~/Content/css/jvectormap/jquery-jvectormap-1.2.2.css",
                //Date Picker 
                     "~/Content/css/datepicker/datepicker3.css",
                //Daterange picker 
                     "~/Content/css/daterangepicker/daterangepicker-bs3.css",
                //bootstrap wysihtml5 - text editor 
                     "~/Content/css/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                //Theme style 
                     "~/Content/css/AdminLTE.css"));
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
