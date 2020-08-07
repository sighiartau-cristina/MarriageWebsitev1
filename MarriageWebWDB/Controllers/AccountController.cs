using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Helper;
using MarriageWebWDB.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR.Messaging;
using Newtonsoft.Json;

namespace MarriageWebWDB.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account

        [HttpGet]
        public ActionResult UserProfile()
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var userProfileId = Session["userProfileId"];
            var address = new AddressHandler().GetForUserProfile((int)userProfileId);

            if (address.CompletedRequest)
            {
                if (address.Entity == null)
                {
                    ViewBag.hasAddress = false;
                }
                else
                {
                    ViewBag.hasAddress = true;
                    ViewBag.AddressStreet = address.Entity.AddressStreet;
                    ViewBag.AddressStreetNo = address.Entity.AddressStreetNo;
                    ViewBag.AddressCity = address.Entity.AddressCity;
                    ViewBag.AddressCountry = address.Entity.AddressCountry;
                }

            }
            else
            {
                TempData["error"] = address.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            var userModel = new UserHelper().GetUserModel();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult UpdateUser(UserModel userModel)
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
            var userProfile = new UserProfileHandler().Get(userProfileId);
            var userHelper = new UserHelper();

            if (userProfile.CompletedRequest)
            {
                if (!userHelper.CheckUpdatedUser(userModel, userProfile.Entity))
                {
                    ViewBag.UpdateUserMessage = userHelper.InvalidInfoMessage;
                    var newUserModel = userHelper.GetUserModel(userModel);
                    var address = new AddressHandler().GetForUserProfile(userProfile.Entity.UserProfileId);

                    if (address.CompletedRequest)
                    {
                        if (address.Entity == null)
                        {
                            ViewBag.hasAddress = false;
                        }
                        else
                        {
                            ViewBag.hasAddress = true;
                            ViewBag.AddressStreet = address.Entity.AddressStreet;
                            ViewBag.AddressStreetNo = address.Entity.AddressStreetNo;
                            ViewBag.AddressCity = address.Entity.AddressCity;
                            ViewBag.AddressCountry = address.Entity.AddressCountry;
                        }

                    }
                    else
                    {
                        TempData["error"] = address.ErrorMessage;
                        return RedirectToAction("Index", "Error");
                    }

                    return View("UserProfile", newUserModel);
                }
            }
            else
            {
                return RedirectToAction("Index", "Error", userProfile.ErrorMessage);
            }

            var userId = (int)Session["userId"];
            var user = new UserHandler().Get(userId);

            var userHandler = new UserHandler();
            var userProfileHandler = new UserProfileHandler();

            //update user profile table first
            var userProfileDataEntity = userHelper.ToDataEntity(userModel, userProfile.Entity);
            var resposeUserProfile = userProfileHandler.Update(userProfileDataEntity);

            if (!resposeUserProfile.CompletedRequest || userProfileDataEntity==null)
            {
                TempData["error"] = resposeUserProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            //update user table after
            var userDataEntity = userHelper.ToDataEntity(userModel, user.Entity);
            var resposeUser = userHandler.Update(userDataEntity);

            if (!resposeUser.CompletedRequest)
            {
                TempData["error"] = resposeUser.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            //if username was changed
            Session["userToken"] = userModel.UserName;

            return RedirectToAction("UserProfile", "Account");
        }

        [HttpGet]
        public ActionResult ChangePassword()
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

        [HttpPost]
        public ActionResult ChangePassword(PasswordModel passwordModel)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            PasswordHelper passwordHelper = new PasswordHelper();

            if (!passwordHelper.CheckPassword((int)Session["userId"], passwordModel))
            {
                ViewBag.UpdatePasswordMessage = passwordHelper.UpdatePasswordMessage;
                return View("ChangePassword");
            }

            var userHandler = new UserHandler();
            var userId = (int)Session["userId"];
            var user = userHandler.Get(userId);

            if (!user.CompletedRequest)
            {
                TempData["error"] = user.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            user.Entity.UserPassword = passwordModel.NewPassword;
            user = userHandler.Update(user.Entity);

            if (!user.CompletedRequest)
            {
                TempData["error"] = user.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("UserProfile", "Account");
        }

        [HttpGet]
        public ActionResult AddAddress()
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

        [HandleError]
        [HttpPost]
        public ActionResult AddAddress(AddressModel addressModel)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            AddressHelper addressHelper = new AddressHelper();

            if (!addressHelper.CheckAddress(addressModel))
            {
                ViewBag.AddressMessage = addressHelper.AddressMessage;
                return View("AddAddress");
            }

            var userProfileId = (int)Session["userProfileId"];
            var addressDataEntity = addressHelper.ToDataEntity(userProfileId, addressModel);
            var response = new AddressHandler().Add(addressDataEntity);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("UserProfile", "Account");
        }

        [HttpGet]
        public ActionResult EditAddress()
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
            var response = new AddressHandler().GetForUserProfile(userProfileId);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            TempData["addressId"] = response.Entity.AddressId;
            TempData.Keep();

            var model = new AddressHelper().GetAddressModel(response.Entity.AddressId);
            return View("EditAddress", model);

        }

        [HttpPost]
        public ActionResult EditAddress(AddressModel addressModel)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            AddressHelper addressHelper = new AddressHelper();
            int addressId = (int)TempData["addressId"];
            TempData.Keep();

            if (!addressHelper.CheckAddress(addressModel))
            {
                ViewBag.AddressMessage = addressHelper.AddressMessage;
                var newAddressModel = addressHelper.GetAddressModel(addressId);
                return View("EditAddress", newAddressModel);
            }

            var userProfileId = (int)Session["userProfileId"];
            var entity = addressHelper.ToDataEntity(addressId, userProfileId, addressModel);

            var response = new AddressHandler().Update(entity);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("UserProfile", "Account");
        }

        [HttpPost]
        public ActionResult DeleteAddress()
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            AddressHandler addressHandler = new AddressHandler();
            int id = (int)TempData["addressId"];

            var response = addressHandler.Delete(id);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("UserProfile", "Account");
        }

        [HttpGet]
        public ActionResult ChangeProfilePicture()
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

        [HttpPost]
        public ActionResult ChangeProfilePicture(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var avatar = new FileEntity
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Avatar,
                    ContentType = upload.ContentType,
                    UserProfileId = (int)Session["userProfileId"]

                };

                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    avatar.Content = reader.ReadBytes(upload.ContentLength);
                }

                var fileHandler = new FileHandler();
                var response = fileHandler.GetByUserId((int)Session["userProfileId"]);

                if (!response.CompletedRequest)
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }

                if (response.Entity == null)
                {
                    response = fileHandler.Add(avatar);
                    if (!response.CompletedRequest)
                    {
                        TempData["error"] = response.ErrorMessage;
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        return RedirectToAction("UserProfile", "Account");
                    }

                }

                avatar.FileId = response.Entity.FileId;
                response = fileHandler.Update(avatar);

                if (!response.CompletedRequest)
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }

                return RedirectToAction("UserProfile", "Account");
            }

            ViewBag.FileMessage = MessageConstants.NoFileSelected;
            return View("ChangeProfilePicture");
        }

        [HttpPost]
        public ActionResult DeleteProfilePicture()
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var fileHandler = new FileHandler();
            var userProfileId = (int)Session["userProfileId"];
            var response = fileHandler.GetByUserId(userProfileId);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if (response.Entity != null)
            {
                response = fileHandler.Delete(response.Entity.FileId);
                if (!response.CompletedRequest)
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }
            }
            
            return RedirectToAction("UserProfile", "Account");
        }

        public string GetAllLikes(string term)
        {
            var likesList = new PreferenceHandler().GetAll();

            if (!likesList.CompletedRequest)
            {
                TempData["error"] = likesList.ErrorMessage;
                RedirectToAction("Index", "Error");
            }

            if (string.IsNullOrEmpty(term))
            {
                return JsonConvert.SerializeObject(likesList.Entity);
            }

            var newlist = likesList.Entity.ToList().FindAll(x => x.Name.ToLowerInvariant().Contains(term.ToLowerInvariant())).ToList();

            return JsonConvert.SerializeObject(newlist);
        }

        public int AddPreference(string name, int id, bool like)
        {
            var handler = new PreferenceHandler();
            int likeId = id;
            //new like preference => add it to likes table
            if (id == 0)
            {
                var entity = new PreferenceEntity
                {
                    Name = name.First().ToString().ToUpper() + name.Substring(1)
                };

                var response = handler.Add(entity);

                if (!response.CompletedRequest)
                {
                    TempData["error"] = response.ErrorMessage;
                    RedirectToAction("Index", "Error");
                }

                likeId = response.Entity.Id;
            }

            //add to user's likes/dislikes

            int userProfileId = (int)Session["userProfileId"];
            var response2 = handler.AddForUser(likeId, userProfileId, like);

            if (!response2.CompletedRequest)
            {
                TempData["error"] = response2.ErrorMessage;
                RedirectToAction("Index", "Error");
            }

            return likeId;
        }

        public void DeletePreference(string id, bool like)
        {
            var handler = new PreferenceHandler();
            int likeId = int.Parse(id);

            int userProfileId = (int)Session["userProfileId"];
            var response = handler.DeleteForUser(likeId, userProfileId, like);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                RedirectToAction("Index", "Error");
            }
        }
    }
}