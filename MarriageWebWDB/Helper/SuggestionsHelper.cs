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
                var gender = new GenderHandler().Get(profile.Entity.GenderId);
                var status = new MaritalStatusHandler().Get(profile.Entity.StatusId);
                var religion = new ReligionHandler().Get(profile.Entity.ReligionId);
                var orientation = new OrientationHandler().Get(profile.Entity.OrientationId);

                if(!profile.CompletedRequest || !user.CompletedRequest || !gender.CompletedRequest || !status.CompletedRequest || !religion.CompletedRequest || !orientation.CompletedRequest) 
                {
                    return null;
                }

                var suggestionModel = new SuggestionModel
                {
                    UserName = user.Entity.UserUsername,
                    Description = string.IsNullOrEmpty(profile.Entity.UserProfileDescription) ? "This user has not provided a description." : profile.Entity.UserProfileDescription,
                    FullName = profile.Entity.UserProfileName + " " + profile.Entity.UserProfileSurname,
                    Age = AgeCalculator.GetDifferenceInYears(profile.Entity.UserProfileBirthday, DateTime.Now).ToString(),
                    Gender = gender.Entity.GenderName,
                    Orientation = orientation.Entity.OrientationName,
                    Religion = religion.Entity.ReligionName,
                    Status = status.Entity.StatusName
                };

                list.Add(suggestionModel);
            }
            return list;
        }
    }
}