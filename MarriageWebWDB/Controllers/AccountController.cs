using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.ActionFilters;
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

            var userHelper = new UserHelper();
            var userModel = userHelper.GetUserModel();

            if(userModel == null)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = userHelper.InvalidInfoMessage.Replace(' ', '-') });
            }

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

            if (!userProfile.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = userProfile.ErrorMessage.Replace(' ', '-') });
            }

            if (!userHelper.CheckUpdatedUser(userModel, userProfile.Entity))
            {
                ViewBag.UpdateUserMessage = userHelper.InvalidInfoMessage;
                var newUserModel = userHelper.GetUserModel(userModel);

                if(newUserModel == null)
                {
                    return RedirectToAction("Index", "Error", new { errorMessage = userHelper.InvalidInfoMessage.Replace(' ', '-') });
                }

            return View("UserProfile", newUserModel);
            }

            var userId = (int)Session["userId"];
            var userHandler = new UserHandler();
            var user = userHandler.Get(userId);

            if (!user.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = user.ErrorMessage.Replace(' ', '-') });
            }

            var userProfileHandler = new UserProfileHandler();

            //update user profile
            var userProfileDataEntity = userHelper.ToDataEntity(userModel, userProfile.Entity);
            userProfile = userProfileHandler.Update(userProfileDataEntity);

            if (!userProfile.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = userProfile.ErrorMessage.Replace(' ', '-') });
            }

            //update user
            var userDataEntity = userHelper.ToDataEntity(userModel, user.Entity);
            user= userHandler.Update(userDataEntity);

            if (!user.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = user.ErrorMessage.Replace(' ', '-') });
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
                return RedirectToAction("Index", "Error", new { errorMessage = user.ErrorMessage.Replace(' ', '-') });
            }

            if (!passwordHelper.CheckPassword(user.Entity, passwordModel))
            {
                ViewBag.UpdatePasswordMessage = passwordHelper.UpdatePasswordMessage;
                return View("ChangePassword");
            }

            user = userHandler.Update(user.Entity);

            if (!user.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = user.ErrorMessage.Replace(' ', '-') });
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
                return RedirectToAction("Index", "Error", new { errorMessage = addressResponse.ErrorMessage.Replace(' ', '-') });
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
                return RedirectToAction("Index", "Error", new { errorMessage = addressResponse.ErrorMessage.Replace(' ', '-') });
            }

            var model = new AddressHelper().GetAddressModel(addressResponse.Entity.AddressId);

            if(model == null)
            {
                return RedirectToAction("Index", "Error", new { errorMessage =MessageConstants.ModelError.Replace(' ', '-') });
            }

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
            var userProfileId = (int)Session["userProfileId"];
            var addressHandler = new AddressHandler();
            var oldAddress = addressHandler.GetForUserProfile(userProfileId);

            if (!oldAddress.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = oldAddress.ErrorMessage.Replace(' ', '-') });
            }

            if (!addressHelper.CheckAddress(addressModel))
            {
                ViewBag.AddressMessage = addressHelper.AddressMessage;
                var newAddressModel = addressHelper.GetAddressModel(oldAddress.Entity.AddressId);

                return View("EditAddress", newAddressModel);
            }

            if (addressHelper.NoChanges(oldAddress.Entity, addressModel))
            {
                ViewBag.AddressMessage = MessageConstants.NoChangesMade;
                var newAddressModel = addressHelper.GetAddressModel(oldAddress.Entity.AddressId);

                return View("EditAddress", newAddressModel);
            }
            
            var entity = addressHelper.ToDataEntity(oldAddress.Entity.AddressId, userProfileId, addressModel);
            var addressResponse = addressHandler.Update(entity);

            if (!addressResponse.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = addressResponse.ErrorMessage.Replace(' ', '-') });
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

            var userProfileId = (int)Session["userProfileId"];
            var addressHandler = new AddressHandler();
            var address = addressHandler.GetForUserProfile(userProfileId);

            if (!address.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = address.ErrorMessage.Replace(' ', '-') });
            }

            var response = addressHandler.Delete(address.Entity.AddressId);

            if (!response.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
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
                var response = fileHandler.GetAvatarForUserProfileId((int)Session["userProfileId"]);

                if (!response.CompletedRequest)
                {
                    return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
                }

                if (response.Entity == null)
                {
                    response = fileHandler.Add(avatar);
                    if (!response.CompletedRequest)
                    {
                        return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
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
                    return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
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
            var response = fileHandler.GetAvatarForUserProfileId(userProfileId);

            if (!response.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
            }

            if (response.Entity != null)
            {
                response = fileHandler.Delete(response.Entity.FileId);
                if (!response.CompletedRequest)
                {
                    return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
                }
            }
            
            return RedirectToAction("UserProfile", "Account");
        }

        [AjaxOnly, AjaxErrorFilter]
        public ActionResult GetAllLikes(string term)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

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
                return RedirectToAction("Index", "Error", new { errorMessage = list.ErrorMessage.Replace(' ', '-') });
            }

            return Content(JsonConvert.SerializeObject(list.Entity));
        }

        [AjaxOnly, AjaxErrorFilter]
        public ActionResult AddPreference(string name, int id, bool like)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

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
                    return RedirectToAction("Index", "Error", new { errorMessage = prefResponse.ErrorMessage.Replace(' ', '-') });
                }

                prefId = prefResponse.Entity.Id;
            }

            //add to user's preferences
            int userProfileId = (int)Session["userProfileId"];
            var response = handler.AddForUser(prefId, userProfileId, like);

            if (!response.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
            }

            return Content(prefId.ToString());
        }

        [AjaxOnly, AjaxErrorFilter]
        public ActionResult DeletePreference(string id, bool like)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var handler = new PreferenceHandler();
            int prefId = int.Parse(id);

            int userProfileId = (int)Session["userProfileId"];
            var response = handler.DeleteForUser(prefId, userProfileId, like);

            if (!response.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
            }

            return new EmptyResult();
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
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

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
                    return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
                }

                return RedirectToAction("UserProfile", "Account");
            }

            ViewBag.FileMessage = MessageConstants.NoFileSelected;
            return View("AddGalleryPicture");
        }

        [AjaxOnly, AjaxErrorFilter]
        public ActionResult DeleteGalleryPicture(int id)
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }

            var response = new FileHandler().Delete(id);

            if (!response.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
            }

            return new EmptyResult();
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
                return RedirectToAction("Index", "Error", new { errorMessage = list.ErrorMessage.Replace(' ', '-') });
            }

            return View(list.Entity);
        }
    }
}