using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.DAL;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult VueIndex()
        {

            return View();
        }
    }
}
