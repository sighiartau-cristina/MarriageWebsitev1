using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Helper;
using MarriageWebWDB.Models;
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

            var userProfileId = (int)Session["userProfileId"];
            var address = new AddressHandler().GetForUserProfile(userProfileId);

            if (!address.CompletedRequest)
            {
                if(string.Equals(ErrorConstants.AddressNotFound, address.ErrorMessage))
                {
                    ViewBag.hasAddress = false;
                }
                else
                {
                    TempData["error"] = address.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }
            }
            else
            {
                ViewBag.hasAddress = true;
                ViewBag.AddressStreet = address.Entity.AddressStreet;
                ViewBag.AddressStreetNo = address.Entity.AddressStreetNo;
                ViewBag.AddressCity = address.Entity.AddressCity;
                ViewBag.AddressCountry = address.Entity.AddressCountry;
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

                    if (!address.CompletedRequest)
                    {
                        if (string.Equals(ErrorConstants.AddressNotFound, address.ErrorMessage))
                        {
                            ViewBag.hasAddress = false;
                        }
                        else
                        {
                            TempData["error"] = address.ErrorMessage;
                            return RedirectToAction("Index", "Error");
                        }
                    }
                    else
                    {
                        ViewBag.hasAddress = true;
                        ViewBag.AddressStreet = address.Entity.AddressStreet;
                        ViewBag.AddressStreetNo = address.Entity.AddressStreetNo;
                        ViewBag.AddressCity = address.Entity.AddressCity;
                        ViewBag.AddressCountry = address.Entity.AddressCountry;
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

            if (!resposeUserProfile.CompletedRequest)
            {
                TempData["error"] = resposeUserProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            //update user table
            var userDataEntity = userHelper.ToDataEntity(userModel, user.Entity);
            var resposeUser = userHandler.Update(userDataEntity);

            if (!resposeUser.CompletedRequest)
            {
                TempData["error"] = resposeUser.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            //in case username was changed
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

            int userId = (int)Session["userId"];
            var userHandler = new UserHandler();
            var user = userHandler.Get(userId);

            if (!user.CompletedRequest)
            {
                TempData["error"] = user.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if (!passwordHelper.CheckPassword(user.Entity, passwordModel))
            {
                ViewBag.UpdatePasswordMessage = passwordHelper.UpdatePasswordMessage;
                return View("ChangePassword");
            }

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
            var addressResponse = new AddressHandler().Add(addressDataEntity);

            if (!addressResponse.CompletedRequest)
            {
                TempData["error"] = addressResponse.ErrorMessage;
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
            var addressResponse = new AddressHandler().GetForUserProfile(userProfileId);

            if (!addressResponse.CompletedRequest)
            {
                TempData["error"] = addressResponse.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            TempData["addressId"] = addressResponse.Entity.AddressId;
            TempData.Keep();

            var model = new AddressHelper().GetAddressModel(addressResponse.Entity.AddressId);
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
            var addressHandler = new AddressHandler();
            var oldAddress = addressHandler.Get(addressId);

            if (!oldAddress.CompletedRequest)
            {
                TempData["error"] = oldAddress.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            if (addressHelper.NoChanges(oldAddress.Entity, addressModel))
            {
                ViewBag.AddressMessage = MessageConstants.NoChangesMade;
                var newAddressModel = addressHelper.GetAddressModel(addressId);
                return View("EditAddress", newAddressModel);
            }

            
            var entity = addressHelper.ToDataEntity(oldAddress.Entity.AddressId, userProfileId, addressModel);
            var addressResponse = addressHandler.Update(entity);

            if (!addressResponse.CompletedRequest)
            {
                TempData["error"] = addressResponse.ErrorMessage;
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

            int id = (int)TempData["addressId"];
            var response = new AddressHandler().Delete(id);

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
                var response = fileHandler.GetForUserProfileId((int)Session["userProfileId"]);

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
            var response = fileHandler.GetForUserProfileId(userProfileId);

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
            ResponseEntity<ICollection<PreferenceEntity>> list;

            if (string.IsNullOrEmpty(term))
            {
                list= new PreferenceHandler().GetAll();
            }
            else
            {
                list = new PreferenceHandler().GetAllForTerm(term);
            }

            if (!list.CompletedRequest)
            {
                TempData["error"] = list.ErrorMessage;
                RedirectToAction("Index", "Error");
            }

            return JsonConvert.SerializeObject(list.Entity);
        }

        public int AddPreference(string name, int id, bool like)
        {
            var handler = new PreferenceHandler();
            int prefId = id;

            //new preference => add it to preferences table
            if (id == 0)
            {
                var entity = new PreferenceEntity
                {
                    Name = name.First().ToString().ToUpper() + name.Substring(1)
                };

                var prefResponse = handler.Add(entity);

                if (!prefResponse.CompletedRequest)
                {
                    TempData["error"] = prefResponse.ErrorMessage;
                    RedirectToAction("Index", "Error");
                }

                prefId = prefResponse.Entity.Id;
            }

            //add to user's preferences
            int userProfileId = (int)Session["userProfileId"];
            var response = handler.AddForUser(prefId, userProfileId, like);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                RedirectToAction("Index", "Error");
            }

            return prefId;
        }

        public void DeletePreference(string id, bool like)
        {
            var handler = new PreferenceHandler();
            int prefId = int.Parse(id);

            int userProfileId = (int)Session["userProfileId"];
            var response = handler.DeleteForUser(prefId, userProfileId, like);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                RedirectToAction("Index", "Error");
            }
        }

        [HttpGet]
        public ActionResult AddGalleryPicture()
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
        public ActionResult AddGalleryPicture(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var pic = new FileEntity
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Gallery,
                    ContentType = upload.ContentType,
                    UserProfileId = (int)Session["userProfileId"]

                };

                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    pic.Content = reader.ReadBytes(upload.ContentLength);
                }

                var fileHandler = new FileHandler();
                var response = fileHandler.Add(pic);

                if (!response.CompletedRequest)
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }

                return RedirectToAction("UserProfile", "Account");
            }

            ViewBag.FileMessage = MessageConstants.NoFileSelected;
            return View("AddGalleryPicture");
        }


        public void DeleteGalleryPicture(int id)
        {
            var fileHandler = new FileHandler();
            fileHandler.Delete(id);
        }

        [HttpGet]
        public ActionResult ManageGallery()
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            int userProfileId = (int)Session["userProfileId"];
            var list = new FileHandler().GetGalleryForUserProfile(userProfileId);

            if (!list.CompletedRequest)
            {
                TempData["error"] = list.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            return View(list.Entity);
        }
    }
}