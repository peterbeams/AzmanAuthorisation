using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Achme.MyApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            return Content("about");
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string data)
        {
            return Content("index.data");
        }
    }
}
