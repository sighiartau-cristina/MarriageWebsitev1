using System;
using System.Web.Mvc;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
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

            return RedirectToAction("Index", "Home");
            
        }

        [HandleError]
        public ActionResult LoginUser(LoginModel loginModel)
        {
            LoginHelper loginHelper = new LoginHelper();
            var response = loginHelper.CheckLogin(loginModel);

            if (!response.CompletedRequest)
            {
                if (!string.IsNullOrEmpty(loginHelper.InvalidLoginMessage))
                {
                    //wrong inputs
                    ViewBag.LoginMessage = loginHelper.InvalidLoginMessage;
                    return View("Login");
                }
                else
                {
                    //other errors
                    return RedirectToAction("Index", "Error", new { errorMessage = response.ErrorMessage.Replace(' ', '-') });
                }
            }

           //get user profile to store in session
            var responseProfile = new UserProfileHandler().GetByUserId(response.Entity.UserId);

            if (!responseProfile.CompletedRequest)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = responseProfile.ErrorMessage.Replace(' ', '-') });
            }

            Session.Add("userToken", loginModel.UserName);
            Session.Add("userId", response.Entity.UserId);
            Session.Add("userProfileId", responseProfile.Entity.UserProfileId);

            return RedirectToAction("Index", "Home");
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
            var registerModel = new RegisterHelper().GetRegisterModel();

            return View(registerModel);
        }

        [HandleError]
        public ActionResult RegisterUser(RegisterModel registerModel)
        {
            RegisterHelper registerHelper = new RegisterHelper();
            var response = registerHelper.AddUser(registerModel);

            if (response)
            {
                //incorrect user input
                if (!string.IsNullOrEmpty(registerHelper.InvalidRegisterMessage))
                {
                    ViewBag.RegisterMessage = registerHelper.InvalidRegisterMessage;
                    ViewBag.Date = DateFormatter.GetDate(DateTime.Now);

                    var model = registerHelper.GetRegisterModel(registerModel);

                    return View("Register", model);
                }
               
                //success
                return RedirectToAction("Login", "Login");
            }

            //errors due to other causes
            return RedirectToAction("Index", "Error", new { errorMessage = registerHelper.InvalidRegisterMessage.Replace(' ', '-') });
        }

        public ActionResult Logout()
        {
            try
            {
                LoginHelper.CheckAccess(Session);
            }
            catch (Exception)
            {
                return View("Login");
            }

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login", "Login");

        }

    }
}