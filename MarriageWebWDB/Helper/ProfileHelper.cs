using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;

namespace MarriageWebWDB.Helper
{
    public class ProfileHelper
    {
        public ProfileModel GetProfileModel(UserEntity user, UserProfileEntity profile, AddressEntity address)
        {

            var gender = new GenderHandler().Get(profile.GenderId);
            var status = new MaritalStatusHandler().Get(profile.StatusId);
            var religion = new ReligionHandler().Get(profile.ReligionId);
            var orientation = new OrientationHandler().Get(profile.OrientationId);
            var file = new FileHandler().GetByUserId(profile.UserProfileId);
            var starsign = new StarSignHandler().Get(profile.StarsignId);

            if (!gender.CompletedRequest || !status.CompletedRequest || !religion.CompletedRequest || !orientation.CompletedRequest || !file.CompletedRequest || !starsign.CompletedRequest)
            {
                return null; 
            }

            var profileModel = new ProfileModel();

            profileModel.UserName = user.UserUsername;
            profileModel.Job = string.IsNullOrEmpty(profile.UserProfileJob) ? "This user has not provided information about their job." : profile.UserProfileJob;
            profileModel.Description = string.IsNullOrEmpty(profile.UserProfileDescription) ? "This user has not provided a description." : profile.UserProfileDescription;
            profileModel.FullName = profile.UserProfileName + " " + profile.UserProfileSurname;
            profileModel.Address = (address == null) ? "This user has not provided information about their address." : address.AddressCity + ", " + address.AddressCountry;
            profileModel.Birthday = DateFormatter.GetDate(profile.UserProfileBirthday);
            profileModel.Age = AgeCalculator.GetDifferenceInYears(profile.UserProfileBirthday, DateTime.Now).ToString();
            profileModel.Gender = gender.Entity.GenderName;
            profileModel.Orientation = orientation.Entity.OrientationName;
            profileModel.Religion = religion.Entity.ReligionName;
            profileModel.Status = status.Entity.StatusName;
            profileModel.File = file.Entity;
            profileModel.Starsign = starsign.Entity.SignName;
            profileModel.Motto = string.IsNullOrEmpty(profile.Motto) ? "-" : profile.Motto;
            profileModel.Likes = string.IsNullOrEmpty(profile.Likes) ? "-" : profile.Likes;
            profileModel.Dislikes = string.IsNullOrEmpty(profile.Dislikes) ? "-" : profile.Dislikes;

            return profileModel;
        }

    }
}