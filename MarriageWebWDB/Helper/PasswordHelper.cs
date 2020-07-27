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

        public bool CheckPassword(int userId, PasswordModel passwordModel)
        {
            PasswordValidator validator = new PasswordValidator();
            var result = validator.Validate(passwordModel);

            if (!result.IsValid)
            {
                UpdatePasswordMessage = ErrorMessageGenerator.ComposeErrorMessage(result);
                return false;
            }

            UserHandler userHandler = new UserHandler();
            ResponseEntity<UserEntity> responseUser = userHandler.Get(userId);

            if (!responseUser.CompletedRequest)
            {
                //TODO change
                UpdatePasswordMessage = "An error occured. Please try again later";
                return false;
            }

            if (!passwordModel.OldPassword.Equals(responseUser.Entity.UserPassword))
            {
                UpdatePasswordMessage = MessageConstants.IncorrectPasswordMessage;
                return false;
            }

            return true;
        }

    }
}