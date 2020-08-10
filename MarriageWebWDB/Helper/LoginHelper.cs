using System;
using System.Web;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using Microsoft.Ajax.Utilities;

namespace MarriageWebWDB.Helper
{
    public class LoginHelper
    {
        public static void CheckAccess(HttpSessionStateBase httpSession)
        {
            if (httpSession["userToken"] == null)
            {
                throw new InvalidOperationException("invalid login token");
            }
        }

        public ResponseEntity<UserEntity> CheckLogin(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = MessageConstants.InvalidLoginMessage
                };
            }

            if (loginModel.UserName.IsNullOrWhiteSpace() || loginModel.UserPassword.IsNullOrWhiteSpace())
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = MessageConstants.MissingFieldsMessage
                };
            }

            UserHandler userHandler = new UserHandler();
            var entity = userHandler.CheckUsernameAndPassword(loginModel.UserName, loginModel.UserPassword);

            if (!entity.CompletedRequest)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = entity.ErrorMessage
                };
            }

            return entity;
        }
    }
}