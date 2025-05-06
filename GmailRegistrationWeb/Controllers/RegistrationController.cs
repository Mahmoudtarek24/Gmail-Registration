using Microsoft.AspNetCore.Mvc.Rendering;

namespace GmailRegistrationWeb.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly GmailDBContext context;
        private readonly PhoneVerification phoneVerification;
        private readonly GenerateEmailSuggestions generateEmail;
        public RegistrationController(GmailDBContext context, GenerateEmailSuggestions generateEmail,PhoneVerification phoneVerification)
        {
            this.context = context;
            this.generateEmail = generateEmail;
            this.phoneVerification = phoneVerification; 
        }

        /// Displays Step 1: Enter First and Last Name. <summary>
        [HttpGet]
        public IActionResult Step1()
        {
            // Retrieve existing first and last names from session, if any
            var model = new RegisterStep1ViewModel()
            {
                FirstName = HttpContext.Session.GetString("FirstName"),
                LastName = HttpContext.Session.GetString("LastName")
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Step1(RegisterStep1ViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            HttpContext.Session.SetString("FirstName", model.FirstName);
            HttpContext.Session.SetString("LastName", model.LastName);
            return RedirectToAction(nameof(Step2));
        }

        [HttpGet]
        public IActionResult Step2()
        {
            var model = new RegisterStep2ViewModel();

            var dateOfBirthString = HttpContext.Session.GetString("DateOfBirth");

            if (!string.IsNullOrEmpty(dateOfBirthString))
            {
                var dateOfBirth = DateTime.Parse(dateOfBirthString);
                model.Month = dateOfBirth.ToString("MMMM");
                model.Day = dateOfBirth.Day;
                model.Year = dateOfBirth.Year;
            }
            var genderString = HttpContext.Session.GetString("Gender");
            if (!string.IsNullOrEmpty(genderString))
            {
                ///convert string value to Enum  ------and leter will convert enum to string when save on database
                model.Gender = Enum.Parse<Gender>("genderString");
            }

            var values = Enum.GetValues(typeof(Gender)).Cast<Gender>();
            model.SelecteGender = values.Select(enumValue => new SelectListItem
            {
                Value = enumValue.ToString(),
                Text = enumValue.ToString(),
            }).ToList();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Step2(RegisterStep2ViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var DateOfBirt = $"{model.Month} {model.Day}, {model.Year}";
            HttpContext.Session.SetString("DateOfBirth", DateOfBirt);
            HttpContext.Session.SetString("Gender", model.Gender.ToString());

            return RedirectToAction(nameof(Step3));
        }
        [HttpGet]
        public IActionResult Step3()
        {
            // Retrieve first and last names from session
            var FirstName = HttpContext.Session.GetString("FirstName");
            var LastName = HttpContext.Session.GetString("LastName");
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                return RedirectToAction(nameof(Step1));

            // Generate Email Baed On Nmae
            var BaseEmail = $"{FirstName.ToLower()}{LastName.ToLower()}@example.com";


            var selectedEmail = HttpContext.Session.GetString("Email");

            var model = new RegisterStep3ViewModel()
            {
                SuggestedEmails = generateEmail.GenerateUniqueEmails(BaseEmail, 3),
                SuggestedEmail = selectedEmail
            };


            // Check if the selected email is one of the suggested emails
            if (model.SuggestedEmails.Contains(model.SuggestedEmail))
            {
                model.SuggestedEmail = selectedEmail;                   //select it if it on new list of sugestion
            }
            else if (!string.IsNullOrEmpty(selectedEmail))
            {
                model.SuggestedEmail = "createOwn";
                model.CustomEmail = selectedEmail;
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Step3(RegisterStep3ViewModel model)
        {

            var FirstName = HttpContext.Session.GetString("FirstName");
            var LastName = HttpContext.Session.GetString("LastName");
            var EmailDmoain = model.Email.Split('@')[1];
            var BaseEmail = $"{FirstName.ToLower()}{LastName.ToLower()}@{EmailDmoain}";

            if (ModelState.IsValid)
            {
                var emailExist = context.users.Any(e => e.Email == model.Email);
                if (emailExist)
                {
                    ModelState.AddModelError(model.Email, "This email address is already taken.");

                    model.SuggestedEmails = generateEmail.GenerateUniqueEmails(BaseEmail, 3);
                    return View(model);
                }
                HttpContext.Session.SetString("Email", model.Email);
                return RedirectToAction(nameof(Step4));

            }
            else
            {
                model.SuggestedEmails = generateEmail.GenerateUniqueEmails(BaseEmail, 3);
                return View(model);
            }
        }

        public IActionResult Step4()
        {
            //will not set getstring becouse when back didnt found enter passsword ,reasign it again
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Step4(RegisterStep4ViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            HttpContext.Session.SetString("Password", model.Password);
            return RedirectToAction(nameof(Step5));
        }
        [HttpGet]
        public IActionResult Step5()
        {
            var model = new RegisterStep5ViewModel()
            {
                CountryCode = HttpContext.Session.GetString("CountryCode"),
                PhoneNumber = HttpContext.Session.GetString("PhoneNumber"),      
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Step5(RegisterStep5ViewModel model)
        {
            //var IsExist=phoneVerification.CheckDuplicatePhoneNumber(model.PhoneNumber);     
            //if(IsExist)
            //{
            //    ModelState.AddModelError(model.PhoneNumber, "This Number Is Used!");
            //    return View(model);
            //}


            if(!ModelState.IsValid) return View(model);

            HttpContext.Session.SetString("CountryCode", model.CountryCode);
            HttpContext.Session.SetString("PhoneNumber", model.PhoneNumber.ToString());

			var  code= phoneVerification.SendVerificationCode(model.PhoneNumber);
            Response.Cookies.Append("VerificationCode", code);
            return RedirectToAction(nameof(Step6));
        }

        [HttpGet]
        public IActionResult Step6()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Step6(RegisterStep6ViewModel model)
        {
            if(!ModelState.IsValid) { return View(model); }

            var PhoneNumber = HttpContext.Session.GetString("PhoneNumber");

            var ValidCode=phoneVerification.ValidateCode(PhoneNumber,model.VerificationCode);
            if (!ValidCode) 
            {
                ModelState.AddModelError(model.VerificationCode, "Not correct Code Please try again");
				TempData["code"] = phoneVerification.SendVerificationCode(PhoneNumber);
                return View(model); 
			}

            return RedirectToAction(nameof(Step7)); 
        }



        [HttpGet]
        public IActionResult Step7()
        {
            var model = new RegisterStep7ViewModel()
            {
                RecoveryEmail = HttpContext.Session.GetString("RecoveryEmail")
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Step7(RegisterStep7ViewModel model)
        {
            if(!ModelState.IsValid) return View(model);
            
            if (!string.IsNullOrEmpty(model.RecoveryEmail))
            {
                HttpContext.Session.SetString("RecoveryEmail", model.RecoveryEmail);
            }       
            return RedirectToAction(nameof(Step8));
        }

        [HttpGet]
        public IActionResult Step8()
        {
            var FirstName = HttpContext.Session.GetString("FirstName");
            var LastName = HttpContext.Session.GetString("LastName");


            var model = new RegisterStep8ViewModel()
            {
                FullName=$"{FirstName} {LastName}",
                Email = HttpContext.Session.GetString("Email"),
                Gender = HttpContext.Session.GetString("Gender"),
                RecoveryEmail = HttpContext.Session.GetString("RecoveryEmail"),
                PhoneNumber=HttpContext.Session.GetString("PhoneNumber")
                
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Step8Confirm()
        {
            return RedirectToAction("step9");
        }

        [HttpGet]
        public IActionResult Step9()
        {
            return View();  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Step9(RegisterStep9ViewModel model)
        {
            if(!ModelState.IsValid) return View(model);



            return RedirectToAction(nameof(Step10)); 
        }
        [HttpGet]
        public IActionResult Step10()
        {
            var FirstName = HttpContext.Session.GetString("FirstName");
            var LastName = HttpContext.Session.GetString("LastName");
			var Email = HttpContext.Session.GetString("Email");
            var Password =HttpContext.Session.GetString("Password");    
            var DateOfBirth = HttpContext.Session.GetString("DateOfBirth");    
            var Gender = HttpContext.Session.GetString("Gender");    
            var RecoveryEmail = HttpContext.Session.GetString("RecoveryEmail");
            var PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            var CountryCode = HttpContext.Session.GetString("CountryCode");


			if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName)|| string.IsNullOrEmpty(Email)||
			   string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(DateOfBirth) || string.IsNullOrEmpty(Gender)
             ||string.IsNullOrEmpty(PhoneNumber)||string.IsNullOrEmpty(CountryCode))
            {
                return RedirectToAction(nameof(Step1));
            }

            var user = new Users
            {
                FirstName= FirstName,
                LastName= LastName,
                Email= Email,
                Password= Password,
                DateOfBirth=DateTime.Parse(DateOfBirth),
                gender= (Gender)Enum.Parse(typeof(Gender), Gender), 
                RecoveryEmail=RecoveryEmail ,
                PhoneNumber=PhoneNumber,    
                CountryCode=CountryCode,               
                
            };
           context.users.Add(user); 
            context.SaveChanges();

			return View();
        }
    }
}
