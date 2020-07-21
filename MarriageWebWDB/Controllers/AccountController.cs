using System;
using System.Collections.Generic;
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
            userModel.Age = MarriageWebWDB.Utils.AgeCalculator.GetDifferenceInYears(userModel.Birthday, DateTime.Now);
            if (!userHelper.CheckUpdatedUser(userModel))
            {
                ViewBag.UpdateUserMessage = userHelper.InvalidInfoMessage;
                var newUserModel = userHelper.GetUserModel(userModel);
                return View("UserProfile", newUserModel);
            }

          // HttpContext context = 
            string username = HttpContext.Session["userToken"].ToString();

            UserEntity user = new UserHandler().GetByUsername(username);
            UserProfileEntity userProfile = new UserProfileHandler().Get(user.UserId);

            UserHandler userHandler = new UserHandler();
            UserProfileHandler userProfileHandler = new UserProfileHandler();

            userProfileHandler.Update(userHelper.ToDataEntity(userModel, userProfile));
            userHandler.Update(userHelper.ToDataEntity(userModel, user));

            return RedirectToAction("Index", "Account");
        }
    }
}