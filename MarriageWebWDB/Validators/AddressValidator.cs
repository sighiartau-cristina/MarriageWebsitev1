using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FluentValidation;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;

namespace MarriageWebWDB.Validators
{
    public class AddressValidator : AbstractValidator<AddressModel> 
    {
        public AddressValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(model => model.Street).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(model => model.StreetNo).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(model => model.City).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().Must(HelperMethods.ValidName).WithMessage(MessageConstants.InvalidCityMessage); ;
            RuleFor(model => model.Country).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().Must(HelperMethods.ValidName).WithMessage(MessageConstants.InvalidCountryMessage); ;

        }

    }
}