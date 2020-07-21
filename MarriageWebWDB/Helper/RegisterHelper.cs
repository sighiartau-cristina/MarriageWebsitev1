using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using Microsoft.Ajax.Utilities;

namespace MarriageWebWDB.Helper
{
    public class RegisterHelper
    {
        public string InvalidRegisterMessage { get; private set; }

        public RegisterModel GetRegisterModel(RegisterModel registerModel=null)
        {
            if (registerModel == null)
            {
                registerModel = new RegisterModel();
            }

            registerModel.Religions = GetReligions();
            registerModel.Statuses = GetStatuses();
            registerModel.Orientations = GetOrientations();
            registerModel.Genders = GetGenders();
            return registerModel;
        }

        public static void CheckAccess(HttpSessionStateBase httpSession)
        {
            if (httpSession["userToken"] != null)
            {
                throw new InvalidOperationException("already logged in");
            }
        }

        private IEnumerable<SelectListItem> GetReligions()
        {
            ReligionHandler religionHandler = new ReligionHandler();
            var religions = religionHandler.GetAll()
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.ReligionId.ToString(),
                                    Text = x.ReligionName
                                });

            return new SelectList(religions, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetGenders()
        {
            GenderHandler genderHandler = new GenderHandler();
            var genders = genderHandler.GetAll()
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.GenderId.ToString(),
                                    Text = x.GenderName
                                });

            return new SelectList(genders, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetOrientations()
        {
            OrientationHandler orientationHandler = new OrientationHandler();
            var orientations = orientationHandler.GetAll()
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.OrientationId.ToString(),
                                    Text = x.OrientationName
                                });

            return new SelectList(orientations, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetStatuses()
        {
            MaritalStatusHandler statusHandler = new MaritalStatusHandler();
            var statuses = statusHandler.GetAll()
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.MaritalStatusId.ToString(),
                                    Text = x.MaritalStatusName
                                });

            return new SelectList(statuses, "Value", "Text");
        }

        public bool CheckRegister(RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                return false;
            }

            //no missing fields
            if (registerModel.UserName.IsNullOrWhiteSpace() ||  registerModel.UserPassword.IsNullOrWhiteSpace() || registerModel.UserConfirmPassword.IsNullOrWhiteSpace() || registerModel.UserEmail.IsNullOrWhiteSpace() || registerModel.UserProfileName.IsNullOrWhiteSpace() || registerModel.UserProfileSurname.IsNullOrWhiteSpace() || registerModel.UserProfileBirthday==null)
            {
                InvalidRegisterMessage = MessageConstants.MissingFieldsMessage;
                return false;
            }

            //valid email address
            if (!CheckValidEmail(registerModel.UserEmail))
            {
                InvalidRegisterMessage = MessageConstants.InvalidEmailMessage;
                return false;
            }

            //valid password
            if (registerModel.UserPassword.Length < 5)
            {
                InvalidRegisterMessage = MessageConstants.InvalidPasswordMessage;
                return false;
            }

            //matching passwords
            if (!registerModel.UserPassword.Equals(registerModel.UserConfirmPassword))
            {
                InvalidRegisterMessage = MessageConstants.PasswordMismatchMessage;
                return false;
            }

            //invalid name
            if (!CheckValidName(registerModel.UserProfileName))
            {
                InvalidRegisterMessage = MessageConstants.InvalidNameMessage;
                return false;
            }

            //invalid surname
            if (!CheckValidName(registerModel.UserProfileSurname))
            {
                InvalidRegisterMessage = MessageConstants.InvalidSurnameMessage;
                return false;
            }

            //invalid age
            int age = GetDifferenceInYears(registerModel.UserProfileBirthday, DateTime.Now);
            if (age<18) 
            {
                InvalidRegisterMessage = MessageConstants.InvalidAgeMessage;
                return false;
            }
            
            UserHandler userHandler = new UserHandler();

            var entity = userHandler.GetByUsernameOrEmail(registerModel.UserName, registerModel.UserEmail);

            if (entity!=null)
            {
                if (entity.UserEmail.Equals(registerModel.UserEmail))
                {
                    InvalidRegisterMessage = MessageConstants.ExistingEmailMessage;
                    return false;
                }
                else
                {
                    InvalidRegisterMessage = MessageConstants.ExistingUsernameMessage;
                    return false;
                }
            }

            var dataEntity = ToDataEntity(registerModel);

            if(dataEntity == null)
            {
                InvalidRegisterMessage = MessageConstants.InvalidRegisterMessage;
                return false;
            }

            int userId = userHandler.Add(dataEntity);

            if(userId == -1)
            {
                InvalidRegisterMessage = MessageConstants.InvalidRegisterMessage;
                return false;
            }

            UserProfileHandler userProfileHandler = new UserProfileHandler();

            var userProfile = ToDataEntity(registerModel, userId);
            int userProfileId = userProfileHandler.Add(userProfile);

            if(userProfileId == -1)
            {
                InvalidRegisterMessage = MessageConstants.InvalidProfileRegisterMessage;
                return false;
            }

            return true;
        }

        private bool CheckValidName(string name)
        {
            var nameNoSpaces = name.Trim();
            if (!Regex.IsMatch(nameNoSpaces, "^([a-z]+[,.]?[ ]?|[a-z]+['-]?)+$"))
            {
               return false;
            }
            return true;
        }

        private bool CheckValidEmail(string email)
        {
            try
            {
                MailAddress address = new MailAddress(email);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        public int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
       
            int years = endDate.Year - startDate.Year;

            if (startDate.Month == endDate.Month && endDate.Day < startDate.Day || endDate.Month < startDate.Month)
            {
                years--;
            }

            return years;
        }

        private UserEntity ToDataEntity(RegisterModel model)
        {
            return new UserEntity
            {
                UserUsername = model.UserName,
                UserPassword = model.UserPassword,
                UserEmail = model.UserEmail,
                CreatedAt = DateTime.Now
            };
        }

        private UserProfileEntity ToDataEntity(RegisterModel model, int userId)
        {
            return new UserProfileEntity
            {
                UserAge = GetDifferenceInYears(model.UserProfileBirthday, DateTime.Now),
                UserProfileBirthday = model.UserProfileBirthday,
                UserProfileName = model.UserProfileName,
                UserProfileSurname = model.UserProfileSurname,
                GenderId = model.GenderId,
                OrientationId = model.OrientationId,
                StatusId = model.StatusId,
                ReligionId = model.ReligionId,
                UserId = userId
            };
        }
    }
}