using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessModel.Handlers;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;

namespace MarriageWebWDB.Helper
{
    public class SuggestionsHelper
    {
        public List<SuggestionModel> GetSuggestions(ICollection<int> ids)
        {
            var list = new List<SuggestionModel>();

            foreach(int id in ids) 
            { 
            
            var profile = new UserProfileHandler().Get(id);
            var user = new UserHandler().Get(profile.Entity.UserId);

                //o sa le mut de aici
                //TODO
            var gender = new GenderHandler().Get(profile.Entity.GenderId);
            var status = new MaritalStatusHandler().Get(profile.Entity.StatusId);
            var religion = new ReligionHandler().Get(profile.Entity.ReligionId);
            var orientation = new OrientationHandler().Get(profile.Entity.OrientationId);
            var file = new FileHandler().GetByUserId(profile.Entity.UserProfileId);

            var suggestionModel = new SuggestionModel();

            suggestionModel.UserName = user.Entity.UserUsername;
            suggestionModel.Description = string.IsNullOrEmpty(profile.Entity.UserProfileDescription) ? "This user has not provided a description." : profile.Entity.UserProfileDescription;
            suggestionModel.FullName = profile.Entity.UserProfileName + " " + profile.Entity.UserProfileSurname;
            suggestionModel.Age = AgeCalculator.GetDifferenceInYears(profile.Entity.UserProfileBirthday, DateTime.Now).ToString();
            suggestionModel.Gender = gender.Entity.GenderName;
            suggestionModel.Orientation = orientation.Entity.OrientationName;
            suggestionModel.Religion = religion.Entity.ReligionName;
            suggestionModel.Status = status.Entity.StatusName;
            suggestionModel.File = file.Entity;

            list.Add(suggestionModel);
            }
            return list;
        }
    }
}