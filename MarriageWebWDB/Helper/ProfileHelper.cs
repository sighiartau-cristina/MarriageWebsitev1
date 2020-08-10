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
            var starsign = new StarSignHandler().Get(profile.StarsignId);

            var likesHandler = new PreferenceHandler();
            var likes = likesHandler.GetAllForUserProfile(profile.UserProfileId, true);
            var dislikes = likesHandler.GetAllForUserProfile(profile.UserProfileId, false);

            if (!gender.CompletedRequest || !status.CompletedRequest || !religion.CompletedRequest || !orientation.CompletedRequest || !starsign.CompletedRequest || !likes.CompletedRequest || !dislikes.CompletedRequest)
            {
                return null; 
            }

            var profileModel = new ProfileModel
            {
                UserName = user.UserUsername,
                Job = string.IsNullOrEmpty(profile.UserProfileJob) ? "This user has not provided information about their job." : profile.UserProfileJob,
                Description = string.IsNullOrEmpty(profile.UserProfileDescription) ? "This user has not provided a description." : profile.UserProfileDescription,
                FullName = profile.UserProfileName + " " + profile.UserProfileSurname,
                Address = (address == null) ? "This user has not provided information about their address." : address.AddressCity + ", " + address.AddressCountry,
                Birthday = DateFormatter.GetDate(profile.UserProfileBirthday),
                Age = AgeCalculator.GetDifferenceInYears(profile.UserProfileBirthday, DateTime.Now).ToString(),
                Gender = gender.Entity.GenderName,
                Orientation = orientation.Entity.OrientationName,
                Religion = religion.Entity.ReligionName,
                Status = status.Entity.StatusName,
                Starsign = starsign.Entity.SignName,
                Motto = string.IsNullOrEmpty(profile.Motto) ? "-" : profile.Motto,
                Likes = likes.Entity.Count > 0 ? likes.Entity.Select(x => x.Name).ToList() : null,
                Dislikes = dislikes.Entity.Count > 0 ? dislikes.Entity.Select(x => x.Name).ToList() : null,
            };

            return profileModel;
        }

    }
}