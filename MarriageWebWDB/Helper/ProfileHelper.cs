using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Models;

namespace MarriageWebWDB.Helper
{
    public class ProfileHelper
    {
        /*public ProfileModel GetProfileModel(UserEntity user, UserProfileEntity profile, AddressEntity address)
        {
            ProfileModel profileModel = new ProfileModel();

            profileModel.UserName = user.UserUsername;
            profileModel.Job = string.IsNullOrEmpty(profile.UserProfileJob) ? "This user has not provided information about their job." : profile.UserProfileJob;
            profileModel.Description = string.IsNullOrEmpty(profile.UserProfileDescription) ? "This user has not provided a description." : profile.UserProfileDescription;

            var gender = new GenderHandler().Get(profile.GenderId);
            if(!gender.CompletedRequest)
        
        }*/

    }
}