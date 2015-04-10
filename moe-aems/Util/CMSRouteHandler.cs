using moe_aems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace moe_aems.Util
{
    public class CMSRouteHandler : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var friendlyUrl = (string)requestContext.RouteData.Values["permalink"];

            CMSRoute routeconfig = CMSRouteDb.Find(friendlyUrl);

            requestContext.RouteData.Values["controller"] = "JQGrid";
            requestContext.RouteData.Values["action"] = "GetList";
            requestContext.RouteData.Values["id"] = routeconfig.ConfigXml;

            return base.GetHttpHandler(requestContext);
        }
    }
}
