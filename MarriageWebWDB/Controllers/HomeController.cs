using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BusinessModel.Constants;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.ActionFilters;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Helper;
using MarriageWebWDB.Models;
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
            var suggestionsList = new UserProfileHandler().GetSuggestions(userProfileId);

            if (!suggestionsList.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = suggestionsList.ErrorMessage.Replace(' ', '-') });
            }

            var models = new SuggestionsHelper().GetSuggestions(suggestionsList.Entity);

            if (models == null)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorConstants.NullEntityError.Replace(' ', '-') });
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
                return RedirectToAction("Index", "Error", new { errorMessage = user.ErrorMessage.Replace(' ', '-') });
            }

            var userProfile = new UserProfileHandler().GetByUserId(user.Entity.UserId);

            if (!userProfile.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = userProfile.ErrorMessage.Replace(' ', '-') });
            }

            var address = new AddressHandler().GetForUserProfile(userProfile.Entity.UserProfileId);

            if(!address.CompletedRequest)
            {
                if(!string.Equals(ErrorConstants.AddressNotFound, address.ErrorMessage))
                {
                    return RedirectToAction("Index", "Error", new { errorMessage = address.ErrorMessage.Replace(' ', '-') });
                }
            }

            var profileModel = new ProfileHelper().GetProfileModel(user.Entity, userProfile.Entity, address.Entity);

            if(profileModel == null)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = MessageConstants.ProfileError.Replace(' ', '-') });
            }
           
            return View("ShowProfile", profileModel);
        }


        [AjaxOnly, AjaxErrorFilter]
        public ActionResult Match(string username, bool accepted)
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
                return RedirectToAction("Index", "Error", new { errorMessage = userProfile.ErrorMessage.Replace(' ', '-') });
            }

            var userProfileToMatch = userProfileHandler.GetByUsername(username);

            if (!userProfileToMatch.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = userProfileToMatch.ErrorMessage.Replace(' ', '-') });
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
                return RedirectToAction("Index", "Error", new { errorMessage = matchResponse.ErrorMessage.Replace(' ', '-') });
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
                return RedirectToAction("Index", "Error", new { errorMessage = matchList.ErrorMessage.Replace(' ', '-') });
            }

            return View(matchList.Entity);
        }

        [AjaxOnly, AjaxErrorFilter]
        public ActionResult GetChatHistory(string username)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var userHandler = new UserHandler();
            var senderId = (int)Session["userId"];
            var receiver = userHandler.GetByUsername(username);

            if (!receiver.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = receiver.ErrorMessage.Replace(' ', '-') });
            }

            var messages = new MessageHandler().GetChatHistory(senderId, receiver.Entity.UserId);

            if (!messages.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = messages.ErrorMessage.Replace(' ', '-') });
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
                return RedirectToAction("Index", "Error", new { errorMessage = userProfileToMatch.ErrorMessage.Replace(' ', '-') });
            }

            if(!matchHandler.Matched(userProfileId, userProfileToMatch.Entity.UserProfileId))
            {
                return RedirectToAction("Index", "Error", new { errorMessage = MessageConstants.ChatNotAvailable.Replace(' ', '-') });
            }

            var response = new MessageHandler().UpdateMessageStatus(userId, userProfileToMatch.Entity.UserId);

            if (!response.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
            }

            ViewBag.toUsername = id;
            return View();
        }

        [AjaxOnly, AjaxErrorFilter]
        public ActionResult ArchiveMessage(string messageId)
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
                return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
            }

            return new EmptyResult();
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
                return RedirectToAction("Index", "Error", new { errorMessage = archivedMessages.ErrorMessage.Replace(' ', '-') });
            }

            var models = new List<MessageModel>();
            var messageHelper = new MessageHelper();

            foreach(MessageEntity entity in archivedMessages.Entity)
            {           
                var userEntity = new UserHandler().Get(entity.ReceiverId);

                if (!userEntity.CompletedRequest)
                {
                    return RedirectToAction("Index", "Error", new { errorMessage = userEntity.ErrorMessage.Replace(' ', '-') });
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
                return RedirectToAction("Index", "Error", new { errorMessage = userProfileToMatch.ErrorMessage.Replace(' ', '-') });
            }

            var matchResponse = new MatchHandler().UnmatchForUsers(userProfileId, userProfileToMatch.Entity.UserProfileId);

            if (!matchResponse.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = matchResponse.ErrorMessage.Replace(' ', '-') });
            }

            var messagesResponse = new MessageHandler().ArchiveAllForUsers(userId, userProfileToMatch.Entity.UserId);

            if (!messagesResponse.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = messagesResponse.ErrorMessage.Replace(' ', '-') });
            }

            return RedirectToAction("Index", "Home");
        }

        [AjaxOnly, AjaxErrorFilter]
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
                return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
            }

            return RedirectToAction("ShowArchivedMessages");
        }

        public ActionResult Gallery(string id)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var userProfile = new UserProfileHandler().GetByUsername(id);

            if (!userProfile.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = userProfile.ErrorMessage.Replace(' ', '-') });
            }

            var list = new FileHandler().GetGalleryForUserProfile(userProfile.Entity.UserProfileId);

            if (!list.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = list.ErrorMessage.Replace(' ', '-') });
            }

            ViewBag.username = id;
            return View("Gallery", list.Entity);
        }

    }
}