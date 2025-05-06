namespace GmailRegistrationWeb.Core.ViewModels
{
    public class RegisterStep6ViewModel
    {
        [Required(ErrorMessage = CustomError.RequiredInput)]
        [RegularExpression(CustomRegix.VerificationCode, ErrorMessage =CustomError.VerificationCode)]
        [Display(Name = "Verification Code")]
        public string VerificationCode { get; set; }
    }
}
