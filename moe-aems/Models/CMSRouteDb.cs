using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace moe_aems.Models
{
    public static class CMSRouteDb
    {
        private static List<CMSRoute> RouteData = null;

        public static void LoadData()
        {
            if (RouteData != null)
            {
                RouteData.Clear();
            }

            var myDeserializer = new XmlSerializer(typeof(List<CMSRoute>));
            using (var myFileStream = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("/Content/xml/CMSRoute.xml"), FileMode.Open))
            {
                RouteData = (List<CMSRoute>)myDeserializer.Deserialize(myFileStream);
            }
        }

        public static bool Exist(string url)
        {
            if (RouteData == null)
            {
                LoadData();
            }

            if (RouteData.Where(x => x.Url.ToLower() == url.ToLower()).Count() > 0 ||
                RouteData.Where(x => (x.Url + "/").ToLower() == url.ToLower()).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public static CMSRoute Find(string url)
        {
            if (RouteData == null)
            {
                LoadData();
            }

            if (RouteData.Where(x => x.Url.ToLower() == url.ToLower()).Count() > 0)
            {
                return RouteData.Where(x => x.Url.ToLower() == url.ToLower()).First();
            }
            else if (RouteData.Where(x => (x.Url + "/").ToLower() == url.ToLower()).Count() > 0)
            {
                return RouteData.Where(x => (x.Url + "/").ToLower() == url.ToLower()).First();
            }
            else
            {
                return null;
            }
            
        }
    }

}
