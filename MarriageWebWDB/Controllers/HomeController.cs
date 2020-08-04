using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
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
        [HttpGet]
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

            return View("Index", models);
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

        public ActionResult Chats(string id)
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

            var response = new MessageHandler().UpdateMessageStatus(userProfileId, userProfileToMatch.Entity.UserProfileId);

            if (!response.CompletedRequest)
            {
                TempData["error"] = ErrorConstants.MessageUpdateError;
                return RedirectToAction("Index", "Error");
            }

            ViewBag.toUsername = id;
            return View();
        }

        public ActionResult ArchiveMessage(string messageId, string username)
        {
            int mId = int.Parse(messageId);
            var response = new MessageHandler().ArchiveMessage(mId);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Chats", new { id = username });
        }

        public ActionResult ShowArchivedMessages()
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
            var archivedMessages = new MessageHandler().GetAllArchivedForSenderId(userProfileId);

            if (!archivedMessages.CompletedRequest)
            {
                TempData["error"] = archivedMessages.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var models = new List<MessageModel>();
            var messageHelper = new MessageHelper();

            //temporary
            foreach(MessageEntity entity in archivedMessages.Entity)
            {
                var userProfileEntity = new UserProfileHandler().Get(entity.ReceiverId);

                if (!userProfileEntity.CompletedRequest)
                {
                    TempData["error"] = userProfileEntity.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }

                var userEntity = new UserHandler().Get(userProfileEntity.Entity.UserId);

                if (!userEntity.CompletedRequest)
                {
                    TempData["error"] = userEntity.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }

                string to = userEntity.Entity.UserUsername;
                string from = Session["userToken"].ToString();

                var model = messageHelper.ConvertToModel(entity, from, to);
                models.Add(model);
            }

            return View(models);
        }

    }
}