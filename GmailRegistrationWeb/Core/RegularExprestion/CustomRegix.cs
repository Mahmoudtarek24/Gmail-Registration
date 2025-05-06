namespace GmailRegistrationWeb.Core.RegularExprestion
{
    public static class CustomRegix
    {
        public const string Name = "^[a-zA-Z]+$";
        public const string password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$!%*?&])[A-Za-z\d@#$!%*?&]{8,}$";
        public const string VerificationCode = @"^\d{5}$";


    }
}
