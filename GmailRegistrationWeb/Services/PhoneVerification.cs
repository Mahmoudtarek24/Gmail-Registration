namespace GmailRegistrationWeb.Services
{
    public class PhoneVerification
    {
        private readonly GmailDBContext context;
        public PhoneVerification(GmailDBContext context)
        {
            this.context = context; 
        }
        //we will simulate phone verification
        private static Dictionary<string, string> _verificationCodes = new Dictionary<string, string>();
        //shared for all instance 

        public string SendVerificationCode(string phoneNumber)
        {
            var code = new Random().Next(23543, 87654).ToString();
            _verificationCodes[phoneNumber] = code;

            Console.WriteLine($"Verification code for {phoneNumber}: {code}");

            return code;
        }

        // Validates the verification code for the specified phone number.
        public bool ValidateCode(string phoneNumber, string code)
        {
            if (_verificationCodes.ContainsKey(phoneNumber))
            {
                if (_verificationCodes[phoneNumber] == code)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckDuplicatePhoneNumber(string phoneNumber)
        {
            if(string.IsNullOrEmpty(phoneNumber)) return false; 

            var phonExist=context.users.Any(e=>e.PhoneNumber== phoneNumber);        
            return phonExist;
        }

    }
}