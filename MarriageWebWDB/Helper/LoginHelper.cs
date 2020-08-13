using System;
using System.Web;
using BusinessModel.Constants;
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
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorConstants.NullEntityError
                };
            }

            if (loginModel.UserName.IsNullOrWhiteSpace() || loginModel.UserPassword.IsNullOrWhiteSpace())
            {
                InvalidLoginMessage = MessageConstants.MissingFieldsMessage;
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                };
            }

            UserHandler userHandler = new UserHandler();
            var response = userHandler.CheckUsernameAndPassword(loginModel.UserName, loginModel.UserPassword);

            if(!response.CompletedRequest && string.Equals(response.ErrorMessage, ErrorConstants.InvalidCredentials))
            {
                InvalidLoginMessage = response.ErrorMessage;
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                };
            }

            return new ResponseEntity<UserEntity>
            {
                CompletedRequest = true,
                Entity = response.Entity
            };
        }
    }
}