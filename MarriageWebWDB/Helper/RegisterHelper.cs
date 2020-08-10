using System;
using System.Web;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;
using MarriageWebWDB.Validators;

namespace MarriageWebWDB.Helper
{
    public class RegisterHelper
    {
        public string InvalidRegisterMessage { get; private set; }

        public RegisterModel GetRegisterModel(RegisterModel registerModel = null)
        {
            if (registerModel == null)
            {
                registerModel = new RegisterModel();
            }

            registerModel.Religions = SelectListGenerator.GetReligions();
            registerModel.Statuses = SelectListGenerator.GetStatuses();
            registerModel.Orientations = SelectListGenerator.GetOrientations();
            registerModel.Genders = SelectListGenerator.GetGenders();

            return registerModel;
        }

        public static void CheckAccess(HttpSessionStateBase httpSession)
        {
            if (httpSession["userToken"] != null)
            {
                throw new InvalidOperationException("already logged in");
            }
        }

        public bool CheckRegister(RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                return false;
            }

            RegisterModelValidator validator = new RegisterModelValidator();

            var result = validator.Validate(registerModel);

            if (!result.IsValid)
            {
                InvalidRegisterMessage = ErrorMessageGenerator.ComposeErrorMessage(result);
                return false;
            }

            return true;
        }

        public bool AddUser(RegisterModel registerModel)
        {

            if (!CheckRegister(registerModel))
            {
                return true;
            }

            var dataEntity = ToDataEntity(registerModel);

            if (dataEntity == null)
            {
                InvalidRegisterMessage = ErrorConstants.NullConvertedEntityError;
                return false;
            }

            var userResponse = new UserHandler().Add(dataEntity);

            if (!userResponse.CompletedRequest)
            {
                InvalidRegisterMessage = userResponse.ErrorMessage;
                return false;
            }

            var userProfile = ToDataEntity(registerModel, userResponse.Entity.UserId);

            if (userProfile==null)
            {
                InvalidRegisterMessage = ErrorConstants.NullConvertedEntityError;
                return false;
            }

            var responseUserProfile = new UserProfileHandler().Add(userProfile);

            if (!responseUserProfile.CompletedRequest)
            {
                InvalidRegisterMessage = responseUserProfile.ErrorMessage;
                return false;   
            }

            return true;
        }

        private UserEntity ToDataEntity(RegisterModel model)
        {
            return new UserEntity
            {
                UserUsername = model.UserName,
                UserPassword = model.UserPassword,
                UserEmail = model.UserEmail,
                CreatedAt = DateTime.Now
            };
        }

        private UserProfileEntity ToDataEntity(RegisterModel model, int userId)
        {
            var starSignName = StarsignCalculator.GetStarsignName(model.UserProfileBirthday);
            var starSignResponse = new StarSignHandler().GetByName(starSignName);

            if(!starSignResponse.CompletedRequest)
            {
                return null;
            }

            return new UserProfileEntity
            {
                UserAge = AgeCalculator.GetDifferenceInYears(model.UserProfileBirthday, DateTime.Now),
                UserProfileBirthday = model.UserProfileBirthday,
                UserProfileName = model.UserProfileName,
                UserProfileSurname = model.UserProfileSurname,
                GenderId = model.GenderId,
                OrientationId = model.OrientationId,
                StatusId = model.StatusId,
                ReligionId = model.ReligionId,
                UserId = userId,
                StarsignId = starSignResponse.Entity.SignId
            };
        }
    }
}