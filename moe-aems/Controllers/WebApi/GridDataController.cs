using moe_aems.Util;
using moe_aems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace moe_aems.Controllers.WebApi
{
    public class GridDataController : ApiController
    {
        private Entities db = new Entities();
        private EntitiesNonLL dbll = new EntitiesNonLL();

        [HttpGet]
        public IHttpActionResult Index(string table)
        {
            JQGridSvc jqgrid = new JQGridSvc();

            if (table == "School")
            {
                return Ok(jqgrid.Query<School>(db, Request));
            }
            else if (table == "SchoolCluster")
            {
                return Ok(jqgrid.Query<SchoolCluster>(db, Request));
            }

            return null;
        }

        [HttpGet]
        public IHttpActionResult Index(string table, string id)
        {
            if (table == "School")
            {
                return Ok(dbll.Schools.Find(int.Parse(id)));
            }

            return null;
        }
         
    }
}
