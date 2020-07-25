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
         public string InvalidLoginMessage { get; private set; }

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
                //InvalidLoginMessage = InvalidLoginMessage;
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = MessageConstants.InvalidLoginMessage
                };
            }

            if (loginModel.UserName.IsNullOrWhiteSpace() || loginModel.UserPassword.IsNullOrWhiteSpace())
            {
                //InvalidLoginMessage = MessageConstants.MissingFieldsMessage;
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
                //InvalidLoginMessage = MessageConstants.InvalidLoginMessage;
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