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

            if (loginHelper.CheckLogin(loginModel))
            {
                Session.Add("userToken", loginModel.UserName);
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