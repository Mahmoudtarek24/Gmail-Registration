namespace GmailRegistrationWeb.Core.ViewModels
{
    public class RegisterStep7ViewModel
    {
        [EmailAddress(ErrorMessage =CustomError.RecoveryEmail )]
        [Display(Name = "Recovery Email (Optional)")]
        public string? RecoveryEmail { get; set; }
    }
}
