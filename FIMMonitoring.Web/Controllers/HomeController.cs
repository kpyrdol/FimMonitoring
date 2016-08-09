using System.Web.Mvc;

namespace FIMMonitoring.Web.Controllers
{
    [Auth(AppRoles = ApplicationRoles.Administrator)]
    public class HomeController : Controller
    {
        [Auth(AppRoles = ApplicationRoles.Administrator)]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Import");
            
        }

    }
}