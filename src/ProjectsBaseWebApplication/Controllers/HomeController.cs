using System.Web.Mvc;

namespace ProjectsBaseWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}