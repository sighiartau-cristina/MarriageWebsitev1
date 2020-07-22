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

        public int CheckLogin(LoginModel loginModel)
        {
            if(loginModel == null)
            {
                return -1;
            }

            if(loginModel.UserName.IsNullOrWhiteSpace() || loginModel.UserPassword.IsNullOrWhiteSpace())
            {
                InvalidLoginMessage = MessageConstants.MissingFieldsMessage;
                return -1;
            }

            UserHandler userHandler = new UserHandler();
            var entity = userHandler.CheckUsernameAndPassword(loginModel.UserName, loginModel.UserPassword);
            
            if(entity == null)
            {
                InvalidLoginMessage = MessageConstants.InvalidLoginMessage;
                return -1;
            }

            return entity.UserId;
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