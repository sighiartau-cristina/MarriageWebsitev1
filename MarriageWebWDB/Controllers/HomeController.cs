using System;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Web.Mvc;
using BusinessModel.Constants;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Helper;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;

namespace MarriageWebWDB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ShowProfile(string id)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var user = new UserHandler().GetByUsername(id);

            if (!user.CompletedRequest)
            {
                TempData["error"] = user.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if (user.Entity == null)
            {
                TempData["error"] = ErrorConstants.UserNotFound;
                return RedirectToAction("Index", "Error");
            }

            var profile = new UserProfileHandler().GetByUserId(user.Entity.UserId);

            if (!profile.CompletedRequest)
            {
                TempData["error"] = profile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if (profile.Entity == null)
            {
                TempData["error"] = ErrorConstants.UserNotFound;
                return RedirectToAction("Index", "Error");
            }

            var address = new AddressHandler().GetForUserProfile(profile.Entity.UserProfileId);

            if(!address.CompletedRequest)
            {
                TempData["error"] = address.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            //aici nu pare prea bine
            //TODO
            var gender = new GenderHandler().Get(profile.Entity.GenderId);
            var status = new MaritalStatusHandler().Get(profile.Entity.StatusId);
            var religion = new ReligionHandler().Get(profile.Entity.ReligionId);
            var orientation = new OrientationHandler().Get(profile.Entity.OrientationId);

            if(!gender.CompletedRequest || !status.CompletedRequest || !religion.CompletedRequest || !orientation.CompletedRequest)
            {
                TempData["error"] = gender.ErrorMessage + status.ErrorMessage + religion.ErrorMessage + orientation.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var profileModel = new ProfileModel();

            profileModel.UserName = user.Entity.UserUsername;
            profileModel.Job = string.IsNullOrEmpty(profile.Entity.UserProfileJob) ? "This user has not provided information about their job." : profile.Entity.UserProfileJob;
            profileModel.Description = string.IsNullOrEmpty(profile.Entity.UserProfileDescription) ? "This user has not provided a description." : profile.Entity.UserProfileDescription;
            profileModel.FullName = profile.Entity.UserProfileName + " " + profile.Entity.UserProfileSurname;
            profileModel.Address = (address == null) ? "This user has not provided information about their address." : address.Entity.AddressStreet + ", " + address.Entity.AddressStreetNo + ", " + address.Entity.AddressCity + ", " + address.Entity.AddressCountry;
            profileModel.Birthday = DateFormatter.GetDate(profile.Entity.UserProfileBirthday);
            profileModel.Age = AgeCalculator.GetDifferenceInYears(profile.Entity.UserProfileBirthday, DateTime.Now).ToString();
            profileModel.Gender = gender.Entity.GenderName;
            profileModel.Orientation = orientation.Entity.OrientationName;
            profileModel.Religion = religion.Entity.ReligionName;
            profileModel.Status = status.Entity.MaritalStatusName;

            profileModel.File = new FileEntityHandler().GetByUserId(profile.Entity.UserProfileId);


            return View("ShowProfile", profileModel);
        }
    }
}