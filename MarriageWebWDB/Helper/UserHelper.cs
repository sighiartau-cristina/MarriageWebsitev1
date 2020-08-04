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
                var responseUserProfileEntity = new UserProfileHandler().GetByUserId(user.UserId);
                if (responseUserProfileEntity.CompletedRequest)
                {
                    var responseFile = new FileHandler().GetByUserId(responseUserProfileEntity.Entity.UserProfileId);
                    if (responseFile.CompletedRequest)
                    {
                        UserProfileEntity userProfile = responseUserProfileEntity.Entity;
                        PopulateModel(userModel, user, userProfile, responseFile.Entity);
                    }
                }
            }

            return userModel;
        }

        private void PopulateModel(UserModel userModel, UserEntity user, UserProfileEntity userProfile, FileEntity file)
        {
            userModel.Religions = SelectListGenerator.GetSelectedReligions(userProfile);
            userModel.Statuses = SelectListGenerator.GetSelectedStatuses(userProfile);
            userModel.Orientations = SelectListGenerator.GetSelectedOrientations(userProfile);
            userModel.Genders = SelectListGenerator.GetSelectedGenders(userProfile);
            userModel.Email = user.UserEmail;
            userModel.UserName = user.UserUsername;
            userModel.Description = string.IsNullOrWhiteSpace(userProfile.UserProfileDescription) ? "" : userProfile.UserProfileDescription;
            userModel.Phone = string.IsNullOrWhiteSpace(userProfile.UserProfilePhone) ? "" : userProfile.UserProfilePhone;
            userModel.Job = string.IsNullOrWhiteSpace(userProfile.UserProfileJob) ? "" : userProfile.UserProfileJob;
            userModel.Name = userProfile.UserProfileName;
            userModel.Surname = userProfile.UserProfileSurname;
            userModel.ReligionId = userProfile.ReligionId;
            userModel.StatusId = userProfile.StatusId;
            userModel.OrientationId = userProfile.OrientationId;
            userModel.GenderId = userProfile.GenderId;
            userModel.Age = AgeCalculator.GetDifferenceInYears(userProfile.UserProfileBirthday, DateTime.Now);
            userModel.Birthday = userProfile.UserProfileBirthday;
            userModel.BirthdayString = DateFormatter.GetDate(userProfile.UserProfileBirthday);
            userModel.File = file;
            userModel.Starsign = StarsignCalculator.GetStarsignName(userProfile.UserProfileBirthday);
            userModel.Likes = string.IsNullOrWhiteSpace(userProfile.Likes) ? "" : userProfile.Likes;
            userModel.Dislikes = string.IsNullOrWhiteSpace(userProfile.Dislikes) ? "" : userProfile.Dislikes;
            userModel.Motto = string.IsNullOrWhiteSpace(userProfile.Motto) ? "" : userProfile.Motto;

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

            if (userModel.Motto != userProfileEntity.Motto)
            {
                return false;
            }

            if (userModel.Likes != userProfileEntity.Likes)
            {
                return false;
            }

            if (userModel.Dislikes != userProfileEntity.Dislikes)
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
            int starSignId = GetStarsignId(StarsignCalculator.GetStarsignName(model.Birthday));

            if (starSignId < 0)
            {
                return null;
            }

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
                UserId = userProfile.UserId,
                Motto = model.Motto,
                Dislikes = model.Dislikes,
                Likes = model.Likes,
                StarsignId = starSignId
            };
        }

        private int GetStarsignId(string name)
        {
            var response = new StarSignHandler().GetByName(name);

            if (!response.CompletedRequest)
            {
                return -1;
            }

            return response.Entity.SignId;
        }
    }
}