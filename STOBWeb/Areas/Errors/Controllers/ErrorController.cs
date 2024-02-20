using STOBWeb.Areas.Home.Controllers;
using System.Web.Mvc;

namespace STOBWeb.Areas.Errors.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Error()
        {
            //sam is my hero
            return View("~/Areas/Errors/Views/Error/Error.cshtml");
        }

        public ActionResult Index()
        {
            return View("~/Areas/Errors/Views/Error/Error.cshtml");
        }

        public ActionResult NotFound()
        {
            return View("~/Areas/Errors/Views/Error/NotFound.cshtml");
        }

        public ActionResult AccessDenied()
        {
            return View("~/Areas/Errors/Views/Error/AccessDenied.cshtml");
        }
    }
}