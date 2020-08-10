using System.Web.Mvc;

namespace MarriageWebWDB.Controllers
{
    public class ErrorController: Controller
    {
        public ActionResult Index()
        {
            string error = TempData["error"].ToString();
            ViewBag.Error = error;

            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}