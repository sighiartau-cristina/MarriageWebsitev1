﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MarriageWebWDB.Constants
{
    public class MessageConstants
    {
        //General messages
        public static string MissingFieldsMessage = "Please fill in all fields.";

        //Login messages
        public static string InvalidLoginMessage = "The username or password is not correct.";

        //Register messages
        public static string InvalidEmailMessage = "Invalid email address.";
        public static string InvalidPasswordMessage = "Passwords must be at least 5 characters long.";
        public static string PasswordMismatchMessage = "Passwords must match.";
        public static string InvalidNameMessage = "Invalid name.";
        public static string InvalidSurnameMessage = "Invalid surname.";
        public static string InvalidAgeMessage = "You must be over 18 years old to join.";
        public static string ExistingUsernameMessage = "This username has already been registered.";
        public static string ExistingEmailMessage = "This email address has already been registered.";
        public static string InvalidRegisterMessage = "Register operation was unsuccessful.";
        public static string InvalidProfileRegisterMessage = "Information about profile could not be saved.";

    }
}