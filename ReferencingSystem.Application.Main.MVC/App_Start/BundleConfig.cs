using System.Web;
using System.Web.Optimization;

namespace ReferencingSystem.Application.Main.MVC
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

            bundles.Add(new ScriptBundle("~/bundles/siteJs").Include(
                     "~/Scripts/SiteJs/jquery.cookie.js",
                     "~/Scripts/SiteJs/velocity.min.js",
                     "~/Scripts/SiteJs/wow.min.js",
                     "~/Scripts/SiteJs/slidebars.js",
                     "~/Scripts/SiteJs/jquery.bxslider.min.js",
                     "~/Scripts/SiteJs/holder.js",
                     "~/Scripts/SiteJs/buttons.js",
                     "~/Scripts/SiteJs/styleswitcher.js",
                     "~/Scripts/SiteJs/jquery.mixitup.min.js",
                     "~/Scripts/SiteJs/circles.min.js",
                     "~/Scripts/SiteJs/syntaxhighlighter/shCore.js",
                     "~/Scripts/SiteJs/syntaxhighlighter/shBrushXml.js",
                     "~/Scripts/SiteJs/syntaxhighlighter/shBrushJScript.js", 
                     "~/Scripts/SiteJs/app.js"));



            bundles.Add(new StyleBundle("~/Content/RsCss").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/preload.css",
                      "~/Content/bootstrap.css",
                      "~/Content/fontawesome/font-awesome.min.css",
                      "~/Content/animate.min.css",
                      "~/Content/slidebars.css",
                      "~/Content/lightbox.css",
                      "~/Content/jquery.bxslider.css",
                      "~/Content/syntaxhighlighter/shCore.css",
                      "~/Content/site.css",
                      "~/Content/width-boxed.css",
                      "~/Content/buttons.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
