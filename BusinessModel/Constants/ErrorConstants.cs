namespace BusinessModel.Constants
{
    public static class ErrorConstants
    {
        public static string NullEntityError = "Entity is null.";
        public static string NullConvertedEntityError = "Entity is null after conversion.";

        public static string UserInsertError = "An error occured while registering user.";
        public static string UserGetError = "An error occured while fetching user info.";
        public static string UserUpdateError = "An error occured while updating the user info.";
        public static string UserDeleteError = "An error occured while deleting the user.";
        public static string UserNotFound = "No user has been found.";

        public static string UserProfileInsertError = "An error occured while registering profile info.";
        public static string UserProfileGetError = "An error occured while fetching user profile info.";
        public static string UserProfileUpdateError = "An error occured while updating the user profile info.";
        public static string UserProfileDeleteError = "An error occured while deleting the user profile info.";
        public static string UserProfileNotFound = "No user profile has been found.";
        public static string UserProfileExisting = "A user profile for this user has already been registered.";

        public static string AddressInsertError = "An error occured while registering address.";
        public static string AddressGetError = "An error occured while fetching address.";
        public static string AddressUpdateError = "An error occured while updating the address info.";
        public static string AddressDeleteError = "An error occured while deleting the address.";
        public static string AddressNotFound = "No address has been found.";
        public static string AddressExisting = "An address for this user has already been registered.";

        public static string OrientationInsertError = "An error occured while registering orientation.";
        public static string OrientationGetError = "An error occured while fetching orientation.";
        public static string OrientationUpdateError = "An error occured while updating orientation.";
        public static string OrientationDeleteError = "An error occured while deleting orientation.";
        public static string OrientationNotFound = "No user has been found.";
        public static string OrientationExisting = "An orientation with this name has already been registered.";

        public static string GenderInsertError = "An error occured while registering gender.";
        public static string GenderGetError = "An error occured while fetching gender.";
        public static string GenderUpdateError = "An error occured while updating gender.";
        public static string GenderDeleteError = "An error occured while deleting gender.";
        public static string GenderNotFound = "No gender has been found.";
        public static string GenderExisting = "A gender with this name has already been registered.";

        public static string MaritalStatusInsertError = "An error occured while registering status.";
        public static string MaritalStatusGetError = "An error occured while fetching status.";
        public static string MaritalStatusUpdateError = "An error occured while updating status.";
        public static string MaritalStatusDeleteError = "An error occured while deleting status.";
        public static string MaritalStatusNotFound = "No status has been found.";
        public static string MaritalStatusExisting = "A status with this name has already been registered.";

        public static string ReligionInsertError = "An error occured while registering religion.";
        public static string ReligionGetError = "An error occured while fetching religion.";
        public static string ReligionUpdateError = "An error occured while updating religion.";
        public static string ReligionDeleteError = "An error occured while deleting religion.";
        public static string ReligionNotFound = "No religion has been found.";
        public static string ReligionExisting = "A religion with the same name has already been registered.";

        public static string MatchInsertError = "An error occured while registering match.";
        public static string MatchGetError = "An error occured while fetching match.";
        public static string MatchUpdateError = "An error occured while updating match.";
        public static string MatchDeleteError = "An error occured while deleting match.";
        public static string MatchNotFound = "No match has been found.";
        public static string MatchExisting = "A match for these profiles has already been registered.";

        public static string ServerError = "Server-side error.";

        public static string ExistingUsernameError = "This username has already been registered.";
        public static string ExistingEmailError = "This email has already been registered.";

        public static string NotFoundEntityForUpdateError = "Entity cannot be found in order to be updated.";

        public static string InvalidCredentials = "Wrong username or password.";

        public static string FileInsertError = "An error occured while registering image file.";
        public static string FileGetError = "An error occured while fetching image file.";
        public static string FileUpdateError = "An error occured while updating the image file.";
        public static string FileDeleteError = "An error occured while deleting the image file.";
        public static string FileNotFound = "No file has been found.";
    }
}
