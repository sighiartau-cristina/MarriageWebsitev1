using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using BusinessModel.Handlers;
using MarriageWebWDB.Utils;

namespace MarriageWebWDB.Validators
{
    public static class HelperMethods
    {

        public static bool UniqueUserName(string username)
        {
            UserHandler userHandler = new UserHandler();
            var entity = userHandler.GetByUsername(username);

            if (entity != null)
            {
                return false;
            }

            return true;

        }

        public static bool Over18Years(DateTime birthday)
        {
            if (AgeCalculator.GetDifferenceInYears(birthday, DateTime.Now) < 18)
            {
                return false;
            }

            return true;
        }

        public static bool ValidName(string name)
        {
            return Regex.IsMatch(name.Trim(), "^([a-zA-Z]+[,.]?[ ]?|[a-zA-Z]+['-]?)+$");
        }
    }
}