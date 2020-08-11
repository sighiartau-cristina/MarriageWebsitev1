using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BusinessModel.Constants;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Helper;
using MarriageWebWDB.Models;

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
            var suggestionsList = new UserProfileHandler().GetSuggestions(userProfileId);

            if (!suggestionsList.CompletedRequest)
            {
                TempData["error"] = suggestionsList.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var models = new SuggestionsHelper().GetSuggestions(suggestionsList.Entity);

            if (models == null)
            {
                TempData["error"] = ErrorConstants.NullEntityError;
                return RedirectToAction("Index", "Error");
            }

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

            var userProfile = new UserProfileHandler().GetByUserId(user.Entity.UserId);

            if (!userProfile.CompletedRequest)
            {
                TempData["error"] = userProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var address = new AddressHandler().GetForUserProfile(userProfile.Entity.UserProfileId);

            if(!address.CompletedRequest)
            {
                if(!string.Equals(ErrorConstants.AddressNotFound, address.ErrorMessage))
                {
                    TempData["error"] = address.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }
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

            var userProfileToMatch = userProfileHandler.GetByUsername(id);

            if (!userProfileToMatch.CompletedRequest)
            {
                TempData["error"] = userProfileToMatch.ErrorMessage;
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

            return View(matchList.Entity);
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

            var messages = new MessageHandler().GetChatHistory(senderId, receiver.Entity.UserId);

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
            var userId = (int)Session["userId"];

            var userProfileToMatch = new UserProfileHandler().GetByUsername(id);
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

            var response = new MessageHandler().UpdateMessageStatus(userId, userProfileToMatch.Entity.UserId);

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
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            int mId = int.Parse(messageId);
            var response = new MessageHandler().ArchiveMessage(mId);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Chat", new { id = username });
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

            var userId = (int)Session["userId"];
            var archivedMessages = new MessageHandler().GetAllArchivedForSenderId(userId);

            if (!archivedMessages.CompletedRequest)
            {
                TempData["error"] = archivedMessages.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var models = new List<MessageModel>();
            var messageHelper = new MessageHelper();

            //TODO hmmm
            foreach(MessageEntity entity in archivedMessages.Entity)
            {           
                var userEntity = new UserHandler().Get(entity.ReceiverId);

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

        public ActionResult Unmatch(string username)
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
            var userId = (int)Session["userId"];

            var userProfileToMatch = userProfileHandler.GetByUsername(username);

            if (!userProfileToMatch.CompletedRequest)
            {
                TempData["error"] = userProfileToMatch.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var matchResponse = new MatchHandler().UnmatchForUsers(userProfileId, userProfileToMatch.Entity.UserProfileId);

            if (!matchResponse.CompletedRequest)
            {
                TempData["error"] = matchResponse.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var messagesResponse = new MessageHandler().ArchiveAllForUsers(userId, userProfileToMatch.Entity.UserId);

            if (!messagesResponse.CompletedRequest)
            {
                TempData["error"] = messagesResponse.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteMessage(string id)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            int mId = int.Parse(id);
            var response = new MessageHandler().Delete(mId);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                RedirectToAction("Index", "Error");
            }

            return RedirectToAction("ShowArchivedMessages");
        }

    }
}