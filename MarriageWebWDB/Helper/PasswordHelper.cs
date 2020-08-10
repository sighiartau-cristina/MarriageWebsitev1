using BusinessModel.Contracts;
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

        public bool CheckPassword(UserEntity user, PasswordModel passwordModel)
        {
            PasswordValidator validator = new PasswordValidator();
            var result = validator.Validate(passwordModel);

            if (!result.IsValid)
            {
                UpdatePasswordMessage = ErrorMessageGenerator.ComposeErrorMessage(result);
                return false;
            }

            if (!passwordModel.OldPassword.Equals(user.UserPassword))
            {
                UpdatePasswordMessage = MessageConstants.IncorrectPasswordMessage;
                return false;
            }

            if (passwordModel.OldPassword.Equals(passwordModel.NewPassword))
            {
                UpdatePasswordMessage = MessageConstants.NoPasswordChange;
                return false;
            }

            user.UserPassword = passwordModel.NewPassword;
            return true;
        }
    }
}