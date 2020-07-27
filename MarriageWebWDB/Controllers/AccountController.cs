using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Helper;
using MarriageWebWDB.Models;

namespace MarriageWebWDB.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
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

            var userProfileId = this.Session["userProfileId"];
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

            UserHelper userHelper = new UserHelper();
            var userModel = userHelper.GetUserModel();
            return View(userModel);
        }

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

            UserHelper userHelper = new UserHelper();
            var userProfileId = this.Session["userProfileId"];
            var userProfile = new UserProfileHandler().Get((int)userProfileId);

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

            var user = new UserHandler().Get((int)(Session["userId"]));

            UserHandler userHandler = new UserHandler();
            UserProfileHandler userProfileHandler = new UserProfileHandler();

            ResponseEntity<UserProfileEntity> resposeUserProfile = userProfileHandler.Update(userHelper.ToDataEntity(userModel, userProfile.Entity));

            if (!resposeUserProfile.CompletedRequest)
            {
                TempData["error"] = resposeUserProfile.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            ResponseEntity<UserEntity> resposeUser = userHandler.Update(userHelper.ToDataEntity(userModel, user.Entity));

            if (!resposeUser.CompletedRequest)
            {
                TempData["error"] = resposeUser.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            Session["userToken"] = userModel.UserName;

            return RedirectToAction("Index", "Account");
        }

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

        public ActionResult SavePassword(PasswordModel passwordModel)
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
            //TODO password update error
            if (!passwordHelper.CheckPassword((int)Session["userId"], passwordModel))
            {
                ViewBag.UpdatePasswordMessage = passwordHelper.UpdatePasswordMessage;
                return View("ChangePassword");
            }

            var userHandler = new UserHandler();
            var user = userHandler.Get((int)Session["userId"]);

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

            return RedirectToAction("Index", "Account");
        }

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
        public ActionResult SaveAddress(AddressModel addressModel)
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

            var response = addressHelper.AddAddress((int)Session["userProfileId"], addressModel);
            if (!response.CompletedRequest)
            {
                if (!string.IsNullOrEmpty(addressHelper.AddressMessage))
                {
                    ViewBag.AddressMessage = addressHelper.AddressMessage;
                    return View("AddAddress");
                }
                else
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Account");
        }

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

            AddressHelper addressHelper = new AddressHelper();
            AddressModel model;

            var response = new AddressHandler().GetForUserProfile((int)Session["userProfileId"]);
            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            TempData["addressId"] = response.Entity.AddressId;
            model = addressHelper.GetAddressModel(response.Entity.AddressId);
            TempData.Keep();
            return View("EditAddress", model);

        }

        public ActionResult SaveEditedAddress(AddressModel addressModel)
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
            int id = (int)TempData["addressId"];
            TempData.Keep();

            if (!addressHelper.CheckAddress(addressModel))
            {
                ViewBag.AddressMessage = addressHelper.AddressMessage;
                var newAddressModel = addressHelper.GetAddressModel(id);
                return View("EditAddress", newAddressModel);
            }

            AddressHandler addressHandler = new AddressHandler();
            var entity = addressHelper.ToDataEntity(id, (int)Session["userProfileId"], addressModel);

            ResponseEntity<AddressEntity> response = addressHandler.Update(entity);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Index", "Account");
        }

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

            ResponseEntity<AddressEntity> response = addressHandler.Delete(id);

            if (!response.CompletedRequest)
            {
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }


            TempData.Keep();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult AddProfilePicture()
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
        public ActionResult AddProfilePicture(HttpPostedFileBase upload)
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

                new FileEntityHandler().Add(avatar);
            }
            return RedirectToAction("Index", "Account");
        }
    }
}