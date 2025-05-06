namespace GmailRegistrationWeb.Core.Errors
{
    public static class CustomError
    {
        public const string RequiredInput = "{0} Required field";
        public const string NameFormat = "{0} must contain only letters.";
        public const string NumberRange = "{0} Should be Between {1} and {2}";
        public const string ValidEmail = "Please enter a valid email address.";
        public const string MatchedPassword = "confirm Password not match password!";
        public const string MinLength = "Length cannot be less than {1} characters";
        public const string password = "Password must include uppercase, lowercase, number, and special character.";
        public const string VerificationCode = "Please enter a valid 6-digit code.";
        public const string RecoveryEmail = "Please enter a valid recovery email address.";
        public const string AgreePolicy = "You must agree to the terms and privacy policy to proceed.";
        public const string notValidEmail = "Please enter a valid email address.";

	}
}
