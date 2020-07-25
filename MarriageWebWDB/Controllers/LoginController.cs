using System;
using System.Web.Mvc;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Helper;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;

namespace MarriageWebWDB.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return View();
            }

            return RedirectToAction("Index", "Account");
            
        }

        [HandleError]
        public ActionResult LoginUser(LoginModel loginModel)
        {
            LoginHelper loginHelper = new LoginHelper();
            var response = loginHelper.CheckLogin(loginModel);

            if (response.CompletedRequest)
            {
                //temporary solution
                var responseProfile = new UserProfileHandler().GetByUserId(response.Entity.UserId);
                if (!responseProfile.CompletedRequest)
                {
                    TempData["error"] = responseProfile.ErrorMessage;
                    return RedirectToAction("Index", "Error");
                }
                Session.Add("userToken", loginModel.UserName);
                Session.Add("userId", response.Entity.UserId);
                Session.Add("userProfileId", responseProfile.Entity.UserProfileId);
                Session["userId"] = response.Entity.UserId;
                Session["userProfileId"] = responseProfile.Entity.UserProfileId;
                return RedirectToAction("Index", "Account");
            }

            ViewBag.LoginMessage = response.ErrorMessage;
            return View("Login");
        }

        public ActionResult Register()
        {
            try
            {
                RegisterHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Account");
            }

            ViewBag.Date = DateFormatter.GetDate(DateTime.Now);
            RegisterHelper registerHelper = new RegisterHelper();
            var registerModel = registerHelper.GetRegisterModel();
            return View(registerModel);
        }

        public ActionResult RegisterUser(RegisterModel registerModel)
        {
            RegisterHelper registerHelper = new RegisterHelper();
            ResponseEntity<UserEntity> response = registerHelper.AddUser(registerModel);

            if (response.CompletedRequest)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                //If the error is caused by incorrect fields
                if (!string.IsNullOrEmpty(registerHelper.InvalidRegisterMessage))
                {
                    ViewBag.RegisterMessage = response.ErrorMessage;
                    ViewBag.Date = DateFormatter.GetDate(DateTime.Now);

                    return View("Register", registerHelper.GetRegisterModel(registerModel));
                }
                else
                {
                    ViewBag.Error = response.ErrorMessage;
                    return RedirectToAction("Error", "Index");
                }
            }
        }
    }
}