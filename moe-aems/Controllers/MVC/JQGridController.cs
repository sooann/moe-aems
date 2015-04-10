using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moe_aems.Controllers.MVC
{
    public class JQGridController : Controller
    {
        // GET: JQGrid
        public ActionResult GetList(string id)
        {
            ViewBag.Title = "Testing JQGrid";
            ViewBag.ConfigXml = id;
            //return View();
            return View("_LayoutList"); 
        }
    }
}