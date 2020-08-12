using System;
using System.Linq;
using System.Web;
using BusinessModel.Constants;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using FluentValidation.Results;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;
using MarriageWebWDB.Validators;
using Newtonsoft.Json;

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
            var responseUser = new UserHandler().Get(id);

            if (!responseUser.CompletedRequest)
            {
                InvalidInfoMessage = responseUser.ErrorMessage;
                return null;
            }

            var user = responseUser.Entity;
            var responseUserProfile = new UserProfileHandler().GetByUserId(user.UserId);

            if (!responseUserProfile.CompletedRequest)
            {
                InvalidInfoMessage = responseUser.ErrorMessage;
                return null;
            }

            var address = new AddressHandler().GetForUserProfile(responseUserProfile.Entity.UserProfileId);

            if (!address.CompletedRequest)
            {
                if(string.Equals(address.ErrorMessage, ErrorConstants.AddressNotFound))
                {
                    userModel.Address = null;
                }
                else
                {
                    InvalidInfoMessage = responseUser.ErrorMessage;
                    return null;
                }
            }
            else
            {
                userModel.Address = address.Entity;
            }

            PopulateModel(userModel, user, responseUserProfile.Entity);

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
            userModel.Starsign = StarsignCalculator.GetStarsignName(userProfile.UserProfileBirthday);
            userModel.Motto = string.IsNullOrWhiteSpace(userProfile.Motto) ? "" : userProfile.Motto;

            var prefHandler = new PreferenceHandler();
            userModel.LikesList =prefHandler.GetAllForUserProfile(userProfile.UserProfileId, true).Entity.ToList();
            userModel.DislikesList = prefHandler.GetAllForUserProfile(userProfile.UserProfileId, false).Entity.ToList();

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
            var starSign = new StarSignHandler().GetByName(StarsignCalculator.GetStarsignName(model.Birthday));

            if (!starSign.CompletedRequest)
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
                StarsignId = starSign.Entity.SignId
            };
        }
    }
}