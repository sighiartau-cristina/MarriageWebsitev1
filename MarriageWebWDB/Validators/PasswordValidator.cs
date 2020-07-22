using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;

namespace MarriageWebWDB.Validators
{
    public class PasswordValidator : AbstractValidator<PasswordModel>
    {
        public PasswordValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(model => model.OldPassword).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(model => model.NewPassword).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().MinimumLength(5).WithMessage(MessageConstants.InvalidPasswordMessage);
            RuleFor(model => model.ConfirmPassword).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().Matches(model => model.NewPassword).WithMessage(MessageConstants.PasswordMismatchMessage);

        }
    }
}