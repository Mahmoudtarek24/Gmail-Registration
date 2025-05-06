namespace GmailRegistrationWeb.Core.ViewModels
{
    public class RegisterStep1ViewModel
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = CustomError.RequiredInput)]
        [RegularExpression(CustomRegix.Name, ErrorMessage = CustomError.NameFormat)] 
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = CustomError.RequiredInput)]
        [RegularExpression(CustomRegix.Name, ErrorMessage = CustomError.NameFormat)]

        public string LastName { get; set; }    
    }
}
