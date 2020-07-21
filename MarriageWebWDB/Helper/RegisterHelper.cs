using System;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using FluentValidation.Results;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;
using MarriageWebWDB.Validators;
using Microsoft.Ajax.Utilities;

namespace MarriageWebWDB.Helper
{
    public class RegisterHelper
    {
        public string InvalidRegisterMessage { get; private set; }

        public RegisterModel GetRegisterModel(RegisterModel registerModel=null)
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
            if(registerModel == null)
            {
                //InvalidRegisterMessage += MessageConstants.InvalidRegisterMessage;
                return false;
            }

            if (!CheckRegister(registerModel))
            {
                //InvalidRegisterMessage = MessageConstants.InvalidRegisterMessage;
                return false;
            }

            UserHandler userHandler = new UserHandler();
            var dataEntity = ToDataEntity(registerModel);

            if (dataEntity == null)
            {
                //InvalidRegisterMessage = MessageConstants.InvalidRegisterMessage;
                return false;
            }

            int userId = userHandler.Add(dataEntity);

            if (userId == -1)
            {
                //InvalidRegisterMessage = MessageConstants.InvalidRegisterMessage;
                return false;
            }

            UserProfileHandler userProfileHandler = new UserProfileHandler();

            var userProfile = ToDataEntity(registerModel, userId);
            int userProfileId = userProfileHandler.Add(userProfile);

            if (userProfileId == -1)
            {
                //InvalidRegisterMessage = MessageConstants.InvalidProfileRegisterMessage;
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
                UserId = userId
            };
        }
    }
}