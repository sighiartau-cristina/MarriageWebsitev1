using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessModel.Handlers;

namespace MarriageWebWDB.Controllers
{
    public class FileController : Controller
    {
        // GET: File

        public ActionResult Index(string id)
        {
            var fileToRetrieve = new FileEntityHandler().Get(int.Parse(id));
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
        
    }
}