using System.Web;
using System.Web.Optimization;

namespace TicketVerkoopVoetbal
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
                      "~/Scripts/moment.min.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-datetimepicker.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/bootstrap-datetimepicker.less",
                      "~/Content/bootstrap-datetimepicker-build.less",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Reservatie").Include(
                      "~/Content/Reservatie.css"));

            bundles.Add(new StyleBundle("~/Content/Tabel").Include(
                      "~/Content/tabel.css"));

            bundles.Add(new StyleBundle("~/Content/ClubDetail").Include(
                      "~/Content/ClubDetail.css"));

            bundles.Add(new StyleBundle("~/Content/ProfielPagina").Include(
                      "~/Content/Profiel.css"));

            bundles.Add(new StyleBundle("~/Content/Site").Include(
                       "~/Content/navbar.css"));


            // datatbles
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include("~/Scripts/DataTables/jquery.dataTables.min.js",
                    "~/Scripts/DataTables/dataTables.bootstrap.min.js",
                    "~/Scripts/DataTables/dataTables.responsive.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/dataTables").Include("~/Content/DataTables/css/dataTables.bootstrap.min.css",
                "~/Content/DataTables/css/responsive.bootstrap.min.css"
                ));
        }
    }
}
