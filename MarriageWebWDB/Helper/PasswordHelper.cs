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

        public ResponseEntity<UserEntity> UpdatePassword(int userId, PasswordModel passwordModel)
        {
            PasswordValidator validator = new PasswordValidator();
            var result = validator.Validate(passwordModel);

            if (!result.IsValid)
            {
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = ErrorMessageGenerator.ComposeErrorMessage(result)
                };
            }

            UserHandler userHandler = new UserHandler();

            ResponseEntity<UserEntity> responseUser = userHandler.Get(userId);

            if (!responseUser.CompletedRequest)
            {
                return responseUser;
            }

            if (!passwordModel.OldPassword.Equals(responseUser.Entity.UserPassword))
            {
                UpdatePasswordMessage = MessageConstants.IncorrectPasswordMessage;
                return new ResponseEntity<UserEntity>
                {
                    CompletedRequest = false,
                    ErrorMessage = MessageConstants.IncorrectPasswordMessage
                };
            }

            responseUser.Entity.UserPassword = passwordModel.NewPassword;
            responseUser = userHandler.Update(responseUser.Entity);

            if (!responseUser.CompletedRequest)
            {
                return responseUser;
            }

            return new ResponseEntity<UserEntity>
            {
                CompletedRequest = true,
                Entity = responseUser.Entity
            };
        }


    }
}