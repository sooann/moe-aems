using moe_aems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace moe_aems.Util
{
    public class CMSRouteConstraint:IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values[parameterName] != null)
            {
                var permalink = values[parameterName].ToString();
                return CMSRouteDb.Exist(permalink);
            }
            return false;
        }
    }

}
