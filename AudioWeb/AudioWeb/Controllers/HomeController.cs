using System.Web.Mvc;

namespace AudioWeb.Controllers
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
    }
}
