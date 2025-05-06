using GmailRegistrationWeb.Core.RegularExprestion;

namespace GmailRegistrationWeb.Core.ViewModels
{
	public class RegisterStep4ViewModel
	{
		[RegularExpression(CustomRegix.password,ErrorMessage =CustomError.password)]
		[Display(Name = "Password")]
		[DataType(DataType.Password)]
		[MinLength(8, ErrorMessage = CustomError.MinLength)]
		public string Password { get; set; }
		[Display(Name =" Confirm Password")]
		[Compare("Password",ErrorMessage =CustomError.MatchedPassword)]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Show Password")]
		public bool ShowPassword { get; set; }//not related to any database properity
	}
}
