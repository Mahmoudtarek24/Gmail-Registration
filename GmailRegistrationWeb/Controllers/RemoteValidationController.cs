using Microsoft.AspNetCore.Mvc;

namespace GmailRegistrationWeb.Controllers
{
    public class RemoteValidationController : Controller
    {
        private readonly GmailDBContext context;
        private readonly GenerateEmailSuggestions generateEmail;

        public RemoteValidationController(GmailDBContext context, GenerateEmailSuggestions generateEmail)
        {
            this.context = context;
            this.generateEmail = generateEmail;
        }
    
        public IActionResult Index()
        {
            return View();
        }

       // Checks if the provided email is available.If not, returns suggestions.
        // Email: The email to validate
        // Returns a JSON result indicating availability or suggestions

        public IActionResult IsEmailAvailable(string CustomEmail)
        {
            if (string.IsNullOrEmpty(CustomEmail))
                return Json(CustomError.RequiredInput);


            var emailAttribute = new EmailAddressAttribute();
            if(!emailAttribute.IsValid(CustomEmail))
            {
                return Json(CustomError.notValidEmail);
            }
            var IsEmailExist=context.users.SingleOrDefault(e=>e.Email.ToLower() == CustomEmail.ToLower());      
            if(IsEmailExist!=null)
            {
                var suggestedEmails = generateEmail.GenerateUniqueEmails(CustomEmail, 3);
                var suggestions = string.Join(", ", suggestedEmails);
                return Json($"This email address is already in use. Try one of these: {suggestions}");
            }
            return Json(true);
            //true means indicate success to jquery 
        }


		public IActionResult IsPhoneAvailable(string phoneNumber)
		{
			if (string.IsNullOrEmpty(phoneNumber))
				return Json(CustomError.RequiredInput);


			var emailAttribute = new PhoneAttribute();  
			if (!emailAttribute.IsValid(phoneNumber))
			{
				return Json("Phone Number Is Not Valid");
			}
			var IsphoneExist = context.users.Any(e => e.PhoneNumber ==phoneNumber);
			if (IsphoneExist)
			{
				
				return Json($"This Phone Number is already in use.");
			}
			return Json(true);
			//true means indicate success to jquery 
		}
	}
}
