using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
            return View();
        }

        public ActionResult LoginUser(LoginModel loginModel)
        {
            LoginHelper loginHelper = new LoginHelper();
            int id = loginHelper.CheckLogin(loginModel);

            if (id>=0)
            {
                //Session.Add("userToken", loginModel.UserName);
                //temporary solution
                Session.Add("userToken", loginModel.UserName);
                Session.Add("userId", id);
                Session.Add("userProfileId", new UserProfileHandler().GetByUserId(id).UserProfileId);
                Session["userId"] = id;
                Session["userProfileId"] = new UserProfileHandler().GetByUserId(id).UserProfileId;
                return RedirectToAction("Index", "Account");
            }

            ViewBag.LoginMessage = loginHelper.InvalidLoginMessage;
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

            if (registerHelper.AddUser(registerModel))
            {
                Session.Add("userToken", registerModel.UserName);
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.RegisterMessage = registerHelper.InvalidRegisterMessage;
            var newRegisterModel = registerHelper.GetRegisterModel(registerModel);
            return View("Register", newRegisterModel);
        }
    }
}