using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using Microsoft.Ajax.Utilities;

namespace MarriageWebWDB.Helper
{
    public class LoginHelper
    {
        public string InvalidLoginMessage { get; private set; }

        public static void CheckAccess(HttpSessionStateBase httpSession)
        {
            if (httpSession["userToken"] == null)
            {
                throw new InvalidOperationException("invalid login token");
            }
        }

        public bool CheckLogin(LoginModel loginModel)
        {
            if(loginModel == null)
            {
                return false;
            }

            if(loginModel.UserName.IsNullOrWhiteSpace() || loginModel.UserPassword.IsNullOrWhiteSpace())
            {
                InvalidLoginMessage = MessageConstants.MissingFieldsMessage;
                return false;
            }

            UserHandler userHandler = new UserHandler();
            var entity = userHandler.CheckUsernameAndPassword(loginModel.UserName, loginModel.UserPassword);
            
            if(entity == null)
            {
                InvalidLoginMessage = MessageConstants.InvalidLoginMessage;
                return false;
            }

            return true;
        }

        private UserEntity ToDataEntity(LoginModel model)
        {
            return new UserEntity
            {
                UserUsername = model.UserName,
                UserPassword = model.UserPassword
            };
        }
    }
}