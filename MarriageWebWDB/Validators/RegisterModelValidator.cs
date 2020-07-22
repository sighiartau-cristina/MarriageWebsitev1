using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using BusinessModel.Handlers;
using FluentValidation;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;

namespace MarriageWebWDB.Validators
{
    public class RegisterModelValidator: AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(model => model.UserEmail).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().EmailAddress().WithMessage(MessageConstants.InvalidEmailMessage);
            RuleFor(model => model.UserName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().Must(u => !u.Any(x => char.IsWhiteSpace(x))).WithMessage(MessageConstants.InvalidUsernameMessage).Must(HelperMethods.UniqueUserName).WithMessage(MessageConstants.ExistingUsernameMessage);
            RuleFor(model => model.UserPassword).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().MinimumLength(5).WithMessage(MessageConstants.InvalidPasswordMessage);
            RuleFor(model => model.UserConfirmPassword).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(model => model.UserPassword).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Matches(model => model.UserConfirmPassword).WithMessage(MessageConstants.PasswordMismatchMessage);
            RuleFor(model => model.UserProfileName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Must(HelperMethods.ValidName).WithMessage(MessageConstants.InvalidNameMessage);  
            RuleFor(model => model.UserProfileSurname).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Must(HelperMethods.ValidName).WithMessage(MessageConstants.InvalidSurnameMessage);
            RuleFor(model => model.ReligionId).NotNull();
            RuleFor(model => model.GenderId).NotNull();
            RuleFor(model => model.OrientationId).NotNull();
            RuleFor(model => model.StatusId).NotNull();
            RuleFor(model => model.UserProfileBirthday).NotNull().Must(HelperMethods.Over18Years).WithMessage(MessageConstants.InvalidAgeMessage); 
        }
    }
}