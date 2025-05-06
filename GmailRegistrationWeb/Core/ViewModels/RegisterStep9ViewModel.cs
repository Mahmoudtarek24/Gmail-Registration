namespace GmailRegistrationWeb.Core.ViewModels
{
    public class RegisterStep9ViewModel
    {
        [Required(ErrorMessage =CustomError.AgreePolicy )]
        [Display(Name = "I agree to the Terms and Privacy Policy")]
        public bool Agree { get; set; }
    }
}
