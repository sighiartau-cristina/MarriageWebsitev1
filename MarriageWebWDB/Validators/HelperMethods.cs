using System;
using System.Text.RegularExpressions;
using BusinessModel.Handlers;
using MarriageWebWDB.Utils;

namespace MarriageWebWDB.Validators
{
    public static class HelperMethods
    {

        public static bool UniqueUserName(string username)
        {
            UserHandler userHandler = new UserHandler();
            return !userHandler.CheckExistingUsername(username);
        }

        public static bool UniqueEmail(string email)
        {
            UserHandler userHandler = new UserHandler();
            return !userHandler.CheckExistingEmail(email);
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