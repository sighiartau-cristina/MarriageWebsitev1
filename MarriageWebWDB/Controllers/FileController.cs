﻿using System;
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
            var fileToRetrieve = new FileHandler().Get(int.Parse(id));

            if (fileToRetrieve.CompletedRequest)
            {
                return File(fileToRetrieve.Entity.Content, fileToRetrieve.Entity.ContentType);
            }

            TempData["Error"] = fileToRetrieve.ErrorMessage;
            return RedirectToAction("Index", "Error");
        }

        public ActionResult UserFile(string id)
        {
            var fileToRetrieve = new FileHandler().GetForUserUsername(id);

            if (fileToRetrieve.CompletedRequest)
            {
                if (fileToRetrieve.Entity != null)
                {
                    return File(fileToRetrieve.Entity.Content, fileToRetrieve.Entity.ContentType);
                }

                return null;
            }

            TempData["Error"] = fileToRetrieve.ErrorMessage;
            return RedirectToAction("Index", "Error");
        }

    }
}