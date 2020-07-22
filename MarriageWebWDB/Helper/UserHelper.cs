﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
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
    public class UserHelper
    {
        public string InvalidInfoMessage { get; private set; }

        public UserModel GetUserModel(UserModel userModel = null)
        {
            if (userModel == null)
            {
                userModel = new UserModel();
            }

            int id = int.Parse(HttpContext.Current.Session["userId"].ToString());
            UserEntity user = new UserHandler().Get(id);
            UserProfileEntity userProfile = new UserProfileHandler().GetByUserId(user.UserId);
            //TODO check user and userProfile

            PopulateModel(userModel, user, userProfile);

            return userModel;
        }

        private void PopulateModel(UserModel userModel, UserEntity user, UserProfileEntity userProfile)
        {
            userModel.Religions = SelectListGenerator.GetSelectedReligions(userProfile);
            userModel.Statuses = SelectListGenerator.GetSelectedStatuses(userProfile);
            userModel.Orientations = SelectListGenerator.GetSelectedOrientations(userProfile);
            userModel.Genders = SelectListGenerator.GetSelectedGenders(userProfile);
            userModel.Email = user.UserEmail;
            userModel.UserName = user.UserUsername;
            userModel.Description = String.IsNullOrWhiteSpace(userProfile.UserProfileDescription) ? "" : userProfile.UserProfileDescription;
            userModel.Phone = String.IsNullOrWhiteSpace(userProfile.UserProfilePhone) ? "" : userProfile.UserProfilePhone;
            userModel.Job = String.IsNullOrWhiteSpace(userProfile.UserProfileJob) ? "" : userProfile.UserProfileJob;
            userModel.Name = userProfile.UserProfileName;
            userModel.Surname = userProfile.UserProfileSurname;
            userModel.ReligionId = userProfile.ReligionId;
            userModel.StatusId = userProfile.StatusId;
            userModel.OrientationId = userProfile.OrientationId;
            userModel.GenderId = userProfile.GenderId;
            userModel.Age = userProfile.UserAge;
            userModel.Birthday = userProfile.UserProfileBirthday;
            userModel.BirthdayString = DateFormatter.GetDate(userProfile.UserProfileBirthday);
        }

        public static void CheckAccess(HttpSessionStateBase httpSession)
        {
            if (httpSession["userToken"] == null)
            {
                throw new InvalidOperationException("not logged in");
            }
        }

        public bool CheckUpdatedUser(UserModel userModel)
        {
            userModel.Age = AgeCalculator.GetDifferenceInYears(userModel.Birthday, DateTime.Now);
            UserModelValidator validator = new UserModelValidator();
            ValidationResult result = validator.Validate(userModel);

            if (!result.IsValid)
            {
                InvalidInfoMessage = ErrorMessageGenerator.ComposeErrorMessage(result);
                return false;
            }

            UserHandler userHandler = new UserHandler();

            var entity = userHandler.GetByUsernameOrEmail(userModel.UserName, userModel.Email);

            /*if (entity != null)
            {
                if (!entity.UserEmail.Equals(userModel.Email))
                {
                    InvalidInfoMessage = MessageConstants.ExistingEmailMessage;
                    return false;
                }

                if(!entity.UserUsername.Equals(userModel.UserName))
                {
                    InvalidInfoMessage = MessageConstants.ExistingUsernameMessage;
                    return false;
                }
            }*/

            return true;
        }

        public UserEntity ToDataEntity(UserModel model, UserEntity user)
        {
            return new UserEntity
            {
                UserId = user.UserId,
                UserUsername = model.UserName,
                UserPassword = user.UserPassword,
                UserEmail = model.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public UserProfileEntity ToDataEntity(UserModel model, UserProfileEntity userProfile)
        {
            return new UserProfileEntity
            {
                UserProfileId = userProfile.UserProfileId,
                UserAge = AgeCalculator.GetDifferenceInYears(model.Birthday, DateTime.Now),
                UserProfileBirthday = model.Birthday,
                UserProfileName = model.Name,
                UserProfileSurname = model.Surname,
                UserProfileDescription = model.Description,
                UserProfileJob = model.Job,
                UserProfilePhone = model.Phone,
                GenderId = model.GenderId,
                OrientationId = model.OrientationId,
                StatusId = model.StatusId,
                ReligionId = model.ReligionId,
                UserId = userProfile.UserId
            };
        }
    }
}