using FluentValidation;
using MarriageWebWDB.Constants;
using MarriageWebWDB.Models;
using Microsoft.Ajax.Utilities;

namespace MarriageWebWDB.Validators
{
    public class LoginValidator: AbstractValidator<LoginModel>
    {
        public LoginValidator()
    {
    }
    }

}