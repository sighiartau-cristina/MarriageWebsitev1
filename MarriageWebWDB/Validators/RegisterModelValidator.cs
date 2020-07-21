using System;
using System.Collections.Generic;
using System.Linq;
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
            RuleFor(model => model.UserName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().Must(u => !u.Any(x => Char.IsWhiteSpace(x))).WithMessage(MessageConstants.InvalidUsernameMessage);
            RuleFor(model => model.UserPassword).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().MinimumLength(5).WithMessage(MessageConstants.InvalidPasswordMessage);
            RuleFor(model => model.UserConfirmPassword).Cascade(CascadeMode.StopOnFirstFailure).NotNull();
            RuleFor(model => model.UserPassword).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Matches(model => model.UserConfirmPassword).WithMessage(MessageConstants.PasswordMismatchMessage);
            RuleFor(model => model.UserProfileName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage(MessageConstants.MissingFieldsMessage); 
            RuleFor(model => model.UserProfileSurname).Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage(MessageConstants.MissingFieldsMessage);
            RuleFor(model => model.UserProfileName.Trim()).Matches("^([a-zA-Z]+[,.]?[ ]?|[a-zA-Z]+['-]?)+$").WithMessage(MessageConstants.InvalidNameMessage);
            RuleFor(model => model.UserProfileSurname.Trim()).Matches("^([a-zA-Z]+[,.]?[ ]?|[a-zA-Z]+['-]?)+$").WithMessage(MessageConstants.InvalidSurnameMessage);
            RuleFor(model => model.ReligionId).NotNull();
            RuleFor(model => model.GenderId).NotNull();
            RuleFor(model => model.OrientationId).NotNull();
            RuleFor(model => model.StatusId).NotNull();
            RuleFor(model => model.UserProfileBirthday).NotNull();

            RuleFor(model => model.UserProfileBirthday).Custom((birthday, context) =>
            {
                if (AgeCalculator.GetDifferenceInYears(birthday, DateTime.Now) < 18)
                {
                    context.AddFailure(MessageConstants.InvalidAgeMessage);
                }
            });

            RuleFor(model => model.UserName).Custom((username, context) =>
            {
                UserHandler userHandler = new UserHandler();
                var entity = userHandler.GetByUsername(username);

                if(entity != null)
                {
                    context.AddFailure(MessageConstants.ExistingUsernameMessage);
                }

            });
        }
    }
}