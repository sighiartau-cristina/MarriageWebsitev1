using System.Web.Mvc;

namespace MarriageWebWDB.Controllers
{
    public class ErrorController: Controller
    {
        public ActionResult Index(string errorMessage)
        {
            ViewBag.Error = errorMessage.Replace("-", " ");
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}