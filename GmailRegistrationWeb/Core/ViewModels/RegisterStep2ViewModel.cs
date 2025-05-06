namespace GmailRegistrationWeb.Core.ViewModels
{
    public class RegisterStep2ViewModel
    {
        [Display(Name ="Month")]
        [Required(ErrorMessage = CustomError.RequiredInput)]
        public string Month { get; set; }
        [Display(Name = "Day")]
        [Range(1,31,ErrorMessage =CustomError.NumberRange)]  
        [Required(ErrorMessage = CustomError.RequiredInput)]
        public int? Day { get; set; }
        [Display(Name = "Year")]
        [Required(ErrorMessage = CustomError.RequiredInput)]
        [Range(1950,2025,ErrorMessage =CustomError.NumberRange)]
        public int? Year { get; set; }

        [Required(ErrorMessage = CustomError.RequiredInput)]
        public Gender? Gender { get; set; }

        public int MaxYear { get; set; }   =DateTime.Now.Year;  

        public IEnumerable<SelectListItem>? SelecteGender { get; set; } 

    }
}
