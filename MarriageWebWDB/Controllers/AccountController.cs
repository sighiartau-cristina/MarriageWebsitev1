using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            //int userProfileId = int.Parse(Session["userProfileId"].ToString());
            var userProfileId = this.Session["userProfileId"] as int?;
            var address = new AddressHandler().GetForUserProfile((int) userProfileId);
 
            if (address == null)
            {
                ViewBag.hasAddress = false;
            }
            else
            {
                ViewBag.hasAddress = true;
                ViewBag.AddressStreet = address.AddressStreet;
                ViewBag.AddressStreetNo = address.AddressStreetNo;
                ViewBag.AddressCity = address.AddressCity;
                ViewBag.AddressCountry = address.AddressCountry;
                ViewBag.AddressId = address.AddressId;
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
     
            if (!userHelper.CheckUpdatedUser(userModel))
            {
                ViewBag.UpdateUserMessage = userHelper.InvalidInfoMessage;
                var newUserModel = userHelper.GetUserModel(userModel);
                return View("UserProfile", newUserModel);
            }

          // HttpContext context = 
            //string username = HttpContext.Session["userToken"].ToString();

            UserEntity user = new UserHandler().Get(int.Parse(Session["userId"].ToString()));
            UserProfileEntity userProfile = new UserProfileHandler().Get(int.Parse(Session["userProfileId"].ToString()));

            UserHandler userHandler = new UserHandler();
            UserProfileHandler userProfileHandler = new UserProfileHandler();

            userProfileHandler.Update(userHelper.ToDataEntity(userModel, userProfile));
            userHandler.Update(userHelper.ToDataEntity(userModel, user));

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

            //string username = HttpContext.Session["userToken"].ToString();

            PasswordHelper passwordHelper = new PasswordHelper();
            if(!passwordHelper.UpdatePassword(int.Parse(Session["userId"].ToString()), passwordModel))
            {
                ViewBag.UpdatePasswordMessage = passwordHelper.UpdatePasswordMessage;
                return View("ChangePassword");
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
                //var newAddressModel = addressHelper.GetAddressModel(id);
                return View("AddAddress");
            }

            
            if(!addressHelper.AddAddress((int)Session["userProfileId"], addressModel))
            {
                ViewBag.AddressMessage = addressHelper.AddressMessage;
                return View("AddAddress");
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
            TempData["addressId"] = new AddressHandler().GetForUserProfile((int)Session["userProfileId"]).AddressId;
            var model = addressHelper.GetAddressModel((int)TempData["addressId"]);
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
            int id = (int) TempData["addressId"];
            TempData.Keep();

            if (!addressHelper.CheckAddress(addressModel))
            {
                ViewBag.AddressMessage = addressHelper.AddressMessage;
                var newAddressModel = addressHelper.GetAddressModel(id);
                return View("EditAddress", newAddressModel);
            }

            AddressHandler addressHandler = new AddressHandler();
            var entity = addressHelper.ToDataEntity(id, int.Parse(Session["userProfileId"].ToString()), addressModel);
            addressHandler.Update(entity);
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
            int id = (int) TempData["addressId"];
            addressHandler.Delete(id);
            TempData.Keep();
            return RedirectToAction("Index", "Account");
        }
    }
}