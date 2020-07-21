using System;
using System.Linq;
using FluentValidation;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using MarriageWebWDB.Utils;

namespace MarriageWebWDB.Validators
{
    public class UserModelValidator: AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(model => model.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(model => model.UserName).NotNull().NotEmpty().Must(u => !u.Any(x => Char.IsWhiteSpace(x)));
            RuleFor(model => model.Phone).NotNull().Length(10).When(model => model.Phone!=null);
            RuleFor(model => model.Name).NotNull();
            RuleFor(model => model.Surname).NotNull();
            RuleFor(model => model.Name.Trim()).NotEmpty().Matches("^([a-zA-Z]+[,.]?[ ]?|[a-zA-Z]+['-]?)+$");
            RuleFor(model => model.Surname.Trim()).NotEmpty().Matches("^([a-zA-Z]+[,.]?[ ]?|[a-zA-Z]+['-]?)+$");
            RuleFor(model => model.ReligionId).NotNull();
            RuleFor(model => model.GenderId).NotNull();
            RuleFor(model => model.OrientationId).NotNull();
            RuleFor(model => model.StatusId).NotNull();

            RuleFor(model => model.Birthday).NotNull().Custom((birthday, context) => {
                if (AgeCalculator.GetDifferenceInYears(birthday, DateTime.Now) < 18)
                {
                    context.AddFailure(MessageConstants.InvalidAgeMessage);
                }
            });
        }
    }
}