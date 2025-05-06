using System.Text.RegularExpressions;

namespace GmailRegistrationWeb.Core.ViewModels
{
    public class RegisterStep5ViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Country code is required.")]
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone Number")]
        [Remote(action: "IsPhoneAvailable",controller: "RemoteValidation")]
        public string PhoneNumber { get; set; }


        public List<SelectListItem>? CountryCodes { get; set; }=GetCountryCodes();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var result=new List<ValidationResult>();        
            if(!IsValidPhoneNumber(PhoneNumber, CountryCode))
            {
                result.Add(new ValidationResult("Please enter a valid phone number", new[] {nameof(PhoneNumber)}));     
            }

            return result;
        }

        private bool IsValidPhoneNumber(string phoneNumber,string CountryCode)
        {
            if(phoneNumber.Length<=0||string.IsNullOrEmpty(CountryCode)) return false;

            string pattern = CountryCode switch
            {
                "+1"=> @"^[2-9]\d{9}$",
                "+44" => @"^7\d{9}$",
                "+20"=> @"^(01\d{9}|0\d{8,9})$",
                "+49"=> @"^\+49\d{10,11}$",
                "86"=> @"^\+861[3-9]\d{9}$",
				_ => @"^\d{7,15}$",
            };

            return Regex.IsMatch(phoneNumber, pattern);
			///check if phone number match her format (country code)
		}

        public static List<SelectListItem> GetCountryCodes()
        {

            return new List<SelectListItem> { new SelectListItem() { Value = "+20", Text = "Egy (+20)" },
            new SelectListItem { Value = "+1", Text = "US (+1)" },
            new SelectListItem { Value = "+1", Text = "CA (+1)" }, 
            new SelectListItem { Value = "+44", Text = "UK (+44)" },
            new SelectListItem { Value = "+49", Text = "Grm (+49)" },
            new SelectListItem { Value = "+86", Text = "chn (+86)" },
            }.OrderBy(e=>e.Text).ToList();
        }
    }
}
