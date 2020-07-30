using System;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Web.Mvc;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Helper;
using MarriageWebWDB.Hubs;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;
using Microsoft.AspNet.SignalR.Messaging;

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

            var userProfileId = (int)Session["userProfileId"];
            var suggestionsList = new UserProfileHandler().GetSuggestions((int)Session["userProfileId"]);

            if (!suggestionsList.CompletedRequest)
            {
                TempData["error"] = suggestionsList.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var models = new SuggestionsHelper().GetSuggestions(suggestionsList.Entity);

            return View(models);
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

            var userProfile = new UserProfileHandler().GetByUserId(user.Entity.UserId);

            if (!userProfile.CompletedRequest)
            {
                TempData["error"] = userProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if (userProfile.Entity == null)
            {
                TempData["error"] = ErrorConstants.UserProfileNotFound;
                return RedirectToAction("Index", "Error");
            }

            var address = new AddressHandler().GetForUserProfile(userProfile.Entity.UserProfileId);

            if(!address.CompletedRequest)
            {
                TempData["error"] = address.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var profileModel = new ProfileHelper().GetProfileModel(user.Entity, userProfile.Entity, address.Entity);

            if(profileModel == null)
            {
                TempData["error"] = MessageConstants.ProfileError;
                return RedirectToAction("Index", "Error");
            }
           
            return View("ShowProfile", profileModel);
        }

        public ActionResult Match(string id, bool accepted)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var userProfileHandler = new UserProfileHandler();

            var userProfileId = (int)Session["userProfileId"];
            var userProfile = userProfileHandler.Get(userProfileId);

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
                Accepted = accepted
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

            var userProfileId = (int)Session["userProfileId"];
            var matchList = new MatchHandler().GetAllForUserProfile(userProfileId);

            if (!matchList.CompletedRequest)
            {
                TempData["error"] = matchList.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var models = new SuggestionsHelper().GetSuggestions(matchList.Entity);

            return View(models);
        }

        public ActionResult CreateMessage(string messageText, string username)
        {

            var receiver = new UserHandler().GetByUsername(username);
            if (!receiver.CompletedRequest)
            {
                TempData["error"] = receiver.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if (receiver.Entity == null)
            {
                TempData["error"] = ErrorConstants.UserNotFound;
                return RedirectToAction("Index", "Error");
            }

            var message = new MessageEntity
            {
                MessageText = messageText,
                SenderId = (int)Session["userId"],
                ReceiverId = receiver.Entity.UserId,
                SendDate = DateTime.Now,
                ReadDate = DateTime.Now,
                Status = MessageStatus.Sent
            };

            var handler = new MessageHandler().Add(message);

            if (!handler.CompletedRequest)
            {
                TempData["error"] = receiver.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            ChatHub.BroadcastData();
            ViewBag.FromUsername = Session["userToken"].ToString();

            return RedirectToAction("Chat", new { id = username });
        }

        [HttpGet]
        public ActionResult GetChatHistory(string username)
        {
            var userHandler = new UserHandler();
            var senderId = (int)Session["userId"];
            var receiver = userHandler.GetByUsername(username);

            if (!receiver.CompletedRequest)
            {
                TempData["error"] = receiver.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if (receiver.Entity == null)
            {
                TempData["error"] = ErrorConstants.UserNotFound;
                return RedirectToAction("Index", "Error");
            }

            var messages = new MessageHandler().GetChatHistory( senderId, receiver.Entity.UserId);

            if (!messages.CompletedRequest)
            {
                TempData["error"] = messages.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }


            string from = Session["userToken"].ToString();
            string to = receiver.Entity.UserUsername;
            var models = new MessageHelper().GetMessageEntities(messages.Entity, from, to);

            ViewBag.username = to;
            return PartialView("_Chat", models);
        }

        public ActionResult Chat(string id)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var matchHandler = new MatchHandler();
            var userProfileId = (int)Session["userProfileId"];
            var userToMatch = new UserHandler().GetByUsername(id);

            if (!userToMatch.CompletedRequest)
            {
                TempData["error"] = userToMatch.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if (userToMatch.Entity == null)
            {
                TempData["error"] = ErrorConstants.UserNotFound;
                return RedirectToAction("Index", "Error");
            }

            var userProfileToMatch = new UserProfileHandler().GetByUserId(userToMatch.Entity.UserId);
            if (!userProfileToMatch.CompletedRequest)
            {
                TempData["error"] = userProfileToMatch.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if(!matchHandler.Matched(userProfileId, userProfileToMatch.Entity.UserProfileId))
            {
                TempData["error"] = MessageConstants.ChatNotAvailable;
                return RedirectToAction("Index", "Error");
            }

            ViewBag.username = id;
            return View();
        }
    }
}