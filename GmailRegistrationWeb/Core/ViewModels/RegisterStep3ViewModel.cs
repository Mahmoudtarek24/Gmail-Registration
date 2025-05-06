namespace GmailRegistrationWeb.Core.Models
{
    public class RegisterStep3ViewModel
    {
        [Required(ErrorMessage = CustomError.RequiredInput)]
        [EmailAddress(ErrorMessage =CustomError.ValidEmail)]

		public string Email { get; set; }
		// Initialize SuggestedEmails to prevent null references
		public List<string> SuggestedEmails { get; set; }=new List<string>();

		// Selected Suggested Email
		public string? SuggestedEmail { get; set; }

		// Custom Email Input 
		[EmailAddress(ErrorMessage = CustomError.ValidEmail)]
		[Remote(action: "IsEmailAvailable",controller: "RemoteValidation")]
		[Display(Name = "Custom Email")]
		public string? CustomEmail { get; set; }
	}
}
