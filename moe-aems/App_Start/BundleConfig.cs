using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace moe_aems.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js"
                        , "~/Scripts/jquery-ui-1.9.2.custom.min.js"
                        , "~/Scripts/jquery.ui.touch-punch.min.js"
                        , "~/Scripts/jquery.dcjqaccordion.2.7.js"
                        , "~/Scripts/jquery.scrollTo.min.js"
                        , "~/Scripts/jquery.nicescroll.js"
                        , "~/Scripts/common-scripts.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqgrid").Include(
                        "~/Scripts/jqgrid/i18n/grid.locale-en.js"
                        ,"~/Scripts/jqgrid/jquery.jqgrid.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/knockoutjs").Include(
                        "~/Scripts/knockout-{version}.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css/jqgrid-redmond").Include(
                        "~/Content/css/jqgrid/ui.jqgrid.css"
                        ,"~/Theme/redmond/jquery-ui-1.10.3.custom.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                      "~/Content/css/bootstrap.css"
                      , "~/Content/css/font-awesome.css"
                      , "~/Content/css/style.css"
                      , "~/Content/css/style-responsive.css"
                      ));

            BundleTable.EnableOptimizations = true;
        }
    }
}