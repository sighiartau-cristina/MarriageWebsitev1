using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;

namespace MarriageWebWDB.Validators
{
    public class UpdatedUserValidator : AbstractValidator<UserModel>
    {
        public UpdatedUserValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(model => model.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(model => model.UserName).NotNull().NotEmpty().Must(u => !u.Any(x => char.IsWhiteSpace(x)));
            RuleFor(model => model.Phone).NotNull().Length(10).When(model => model.Phone != null);
            RuleFor(model => model.Name).NotNull().NotEmpty().Must(HelperMethods.ValidName).WithMessage(MessageConstants.InvalidNameMessage);
            RuleFor(model => model.Surname).NotNull().NotEmpty().Must(HelperMethods.ValidName).WithMessage(MessageConstants.InvalidSurnameMessage);
            RuleFor(model => model.Birthday).NotNull().Must(HelperMethods.Over18Years).WithMessage(MessageConstants.InvalidAgeMessage);
            RuleFor(model => model.ReligionId).NotNull();
            RuleFor(model => model.GenderId).NotNull();
            RuleFor(model => model.OrientationId).NotNull();
            RuleFor(model => model.StatusId).NotNull();
        }
    }
}