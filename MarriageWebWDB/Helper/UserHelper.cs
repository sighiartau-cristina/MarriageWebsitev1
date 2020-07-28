﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Web;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using FluentValidation.Results;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;
using MarriageWebWDB.Validators;

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
            ResponseEntity<UserEntity> responseUserEntity = new UserHandler().Get(id);

            if (responseUserEntity.CompletedRequest)
            {
                UserEntity user = responseUserEntity.Entity;
                ResponseEntity<UserProfileEntity> responseUserProfileEntity = new UserProfileHandler().GetByUserId(user.UserId);
                if (responseUserProfileEntity.CompletedRequest)
                {
                    UserProfileEntity userProfile = responseUserProfileEntity.Entity;
                    PopulateModel(userModel, user, userProfile);
                }
            }

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

        public bool CheckUpdatedUser(UserModel userModel, UserProfileEntity userProfileEntity)
        {
            userModel.Age = AgeCalculator.GetDifferenceInYears(userModel.Birthday, DateTime.Now);

            var userHandler = new UserHandler();
            var userEntity = userHandler.Get(userProfileEntity.UserId);

            if (!userEntity.CompletedRequest || userEntity.Entity == null)
            {
                return false;
            }

            if (NoChanges(userModel, userProfileEntity, userEntity.Entity))
            {
                InvalidInfoMessage = MessageConstants.NoChangesMade;
                return false;
            }

            var validator = new UpdatedUserValidator();
            ValidationResult result = validator.Validate(userModel);

            if (!result.IsValid)
            {
                InvalidInfoMessage = ErrorMessageGenerator.ComposeErrorMessage(result);
                return false;
            }

            if (userModel.Email != userEntity.Entity.UserEmail && userHandler.CheckExistingEmail(userModel.Email))
            {
                InvalidInfoMessage = MessageConstants.ExistingEmailMessage;
                return false;
            }

            if (userModel.UserName != userEntity.Entity.UserUsername && userHandler.CheckExistingUsername(userModel.UserName))
            {
                InvalidInfoMessage = MessageConstants.ExistingUsernameMessage;
                return false;
            }

            return true;
        }

        public bool NoChanges(UserModel userModel, UserProfileEntity userProfileEntity, UserEntity userEntity)
        {
            if (userModel.Email != userEntity.UserEmail)
            {
                return false;
            }

            if (userModel.UserName != userEntity.UserUsername)
            {
                return false;
            }

            if (userModel.Description != userProfileEntity.UserProfileDescription)
            {
                return false;
            }

            if (userModel.Phone != userProfileEntity.UserProfilePhone)
            {
                return false;
            }

            if (userModel.Name != userProfileEntity.UserProfileName)
            {
                return false;
            }

            if (userModel.Surname != userProfileEntity.UserProfileSurname)
            {
                return false;
            }

            if (userModel.Job != userProfileEntity.UserProfileJob)
            {
                return false;
            }

            if (userModel.OrientationId != userProfileEntity.OrientationId)
            {
                return false;
            }

            if (userModel.ReligionId != userProfileEntity.ReligionId)
            {
                return false;
            }

            if (userModel.StatusId != userProfileEntity.StatusId)
            {
                return false;
            }

            if (userModel.GenderId != userProfileEntity.GenderId)
            {
                return false;
            }

            if (userModel.Birthday != userProfileEntity.UserProfileBirthday)
            {
                return false;
            }

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

        public List<UserModel> Users(ICollection<int> ids)
        {
            List<UserModel> models = new List<UserModel>();
            foreach(int id in ids)
            {
                UserModel userm = new UserModel();
                var profile = new UserProfileHandler().Get(id);
                var user = new UserHandler().Get(profile.Entity.UserId);
                PopulateModel(userm, user.Entity, profile.Entity);
                models.Add(userm);
            }
            return models;
        }
    }
}