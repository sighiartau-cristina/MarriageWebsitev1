using System;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Web.Mvc;
using BusinessModel.Constants;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Helper;
using MarriageWebWDB.Models;
using MarriageWebWDB.SignalRChat;
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

            var list = new UserProfileHandler().GetSuggestions((int)Session["userProfileId"]);
            var models = new SuggestionsHelper().GetSuggestions(list.Entity);

            return View(models);
        }

        public ActionResult Chat()
        {
            var start = new Startup(); 

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

            //o sa le mut de aici
            //TODO
            var gender = new GenderHandler().Get(profile.Entity.GenderId);
            var status = new MaritalStatusHandler().Get(profile.Entity.StatusId);
            var religion = new ReligionHandler().Get(profile.Entity.ReligionId);
            var orientation = new OrientationHandler().Get(profile.Entity.OrientationId);
            var file = new FileHandler().GetByUserId(profile.Entity.UserProfileId);

            if(!gender.CompletedRequest || !status.CompletedRequest || !religion.CompletedRequest || !orientation.CompletedRequest || !file.CompletedRequest)
            {
                TempData["error"] = gender.ErrorMessage + status.ErrorMessage + religion.ErrorMessage + orientation.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var profileModel = new ProfileModel();

            profileModel.UserName = user.Entity.UserUsername;
            profileModel.Job = string.IsNullOrEmpty(profile.Entity.UserProfileJob) ? "This user has not provided information about their job." : profile.Entity.UserProfileJob;
            profileModel.Description = string.IsNullOrEmpty(profile.Entity.UserProfileDescription) ? "This user has not provided a description." : profile.Entity.UserProfileDescription;
            profileModel.FullName = profile.Entity.UserProfileName + " " + profile.Entity.UserProfileSurname;
            profileModel.Address = (address.Entity == null) ? "This user has not provided information about their address." : address.Entity.AddressStreet + ", " + address.Entity.AddressStreetNo + ", " + address.Entity.AddressCity + ", " + address.Entity.AddressCountry;
            profileModel.Birthday = DateFormatter.GetDate(profile.Entity.UserProfileBirthday);
            profileModel.Age = AgeCalculator.GetDifferenceInYears(profile.Entity.UserProfileBirthday, DateTime.Now).ToString();
            profileModel.Gender = gender.Entity.GenderName;
            profileModel.Orientation = orientation.Entity.OrientationName;
            profileModel.Religion = religion.Entity.ReligionName;
            profileModel.Status = status.Entity.StatusName;
            profileModel.File = file.Entity;

            return View("ShowProfile", profileModel);
        }

        public ActionResult MakeMatch(string id)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            //TODO mai bine adaug o proprietate de UserProfileId?
            var userProfileHandler = new UserProfileHandler();
            var userProfile = userProfileHandler.Get((int)Session["userProfileId"]);

            if (!userProfile.CompletedRequest)
            {
                TempData["error"] = userProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var userToMatch = new UserHandler().GetByUsername(id);

            if (!userToMatch.CompletedRequest)
            {
                TempData["error"] = userProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var userProfileToMatch = userProfileHandler.GetByUserId(userToMatch.Entity.UserId);

            if (!userProfileToMatch.CompletedRequest)
            {
                TempData["error"] = userProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var match = new MatchEntity
            {
                MatchDate = DateTime.Now,
                MatchUserProfileId = userProfileToMatch.Entity.UserProfileId,
                UserProfileId = userProfile.Entity.UserProfileId,
                Accepted = true
            };

            var matchResponse = new MatchHandler().Add(match);

            if (!matchResponse.CompletedRequest)
            {
                TempData["error"] = matchResponse.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult RejectMatch(string id)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            //TODO mai bine adaug o proprietate de UserProfileId?
            var userProfileHandler = new UserProfileHandler();
            var userProfile = userProfileHandler.Get((int)Session["userProfileId"]);

            if (!userProfile.CompletedRequest)
            {
                TempData["error"] = userProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var userToMatch = new UserHandler().GetByUsername(id);

            if (!userToMatch.CompletedRequest)
            {
                TempData["error"] = userProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var userProfileToMatch = userProfileHandler.GetByUserId(userToMatch.Entity.UserId);

            if (!userProfileToMatch.CompletedRequest)
            {
                TempData["error"] = userProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var match = new MatchEntity
            {
                MatchDate = DateTime.Now,
                MatchUserProfileId = userProfileToMatch.Entity.UserProfileId,
                UserProfileId = userProfile.Entity.UserProfileId,
                Accepted = false
            };

            var matchResponse = new MatchHandler().Add(match);

            if (!matchResponse.CompletedRequest)
            {
                TempData["error"] = matchResponse.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SeeMatches()
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var list = new MatchHandler().GetAllForUserProfile((int)Session["userProfileId"]);
            var models = new SuggestionsHelper().GetSuggestions(list.Entity);

            return View(models);
        }
    }
}