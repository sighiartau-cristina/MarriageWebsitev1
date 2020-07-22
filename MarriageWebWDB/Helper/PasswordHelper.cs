using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;
using MarriageWebWDB.Validators;

namespace MarriageWebWDB.Helper
{ 
    public class PasswordHelper
    {
        public string UpdatePasswordMessage { get; private set; }
        public bool UpdatePassword(int userId, PasswordModel passwordModel)
        {
            PasswordValidator validator = new PasswordValidator();
            var result = validator.Validate(passwordModel);

            if (!result.IsValid)
            {
                UpdatePasswordMessage = ErrorMessageGenerator.ComposeErrorMessage(result);
                return false;
            }

            UserHandler userHandler = new UserHandler();
            UserEntity user = userHandler.Get(userId);

            if (user == null)
            {
                return false;
            }

            if (!passwordModel.OldPassword.Equals(user.UserPassword))
            {
                UpdatePasswordMessage = MessageConstants.IncorrectPasswordMessage;
                return false;
            }

            user.UserPassword = passwordModel.NewPassword;
            userHandler.Update(user);

            return true;
        }


    }
}