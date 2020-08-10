using FluentValidation.Results;

namespace MarriageWebWDB.Utils
{
    public static class ErrorMessageGenerator
    {
        public static string ComposeErrorMessage(ValidationResult result)
        {
            string errorMessage = "";

            foreach (var failure in result.Errors)
            {
                errorMessage += failure.ErrorMessage;
            }

            return errorMessage;
        }
    }
}