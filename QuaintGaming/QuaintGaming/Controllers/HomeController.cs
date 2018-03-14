using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuaintGaming.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Landing page.
            return View();
        }

        public ActionResult About()
        {
            //About the website.
            return View();
        }

        public ActionResult Links()
        {
            //Links to outside websites.
            return View();
        }
    }
}