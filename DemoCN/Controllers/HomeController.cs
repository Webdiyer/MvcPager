using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace Webdiyer.MvcPagerDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int id = 1)
        {
            return View();
        }

        public ActionResult DemoSelector()
        {
            return View();
        }
    }
}
