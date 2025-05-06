namespace GmailRegistrationWeb.Services
{
    public class GenerateEmailSuggestions
    {
        //service provides methods to create unique email suggestions by appending random numbers,
        //ensuring uniqueness in the database

        private readonly GmailDBContext context;

        public GenerateEmailSuggestions(GmailDBContext context)
        {
            this.context = context; 
        }

        public List<string> GenerateUniqueEmails(string baseEmail, int count = 2)
        {
            var SuggestionEmail =new List<string>();

            // will return null to check every thing is okay
            if(baseEmail is null ||!baseEmail.Contains("@")) 
                return SuggestionEmail; 

            var prefixemail = baseEmail.Split('@')[0];   
            var postfixemail = baseEmail.Split('@')[1];

            string checkEmail = default;
            while(SuggestionEmail.Count <count)
            { 
                var random=new Random();    
                var randomNumber=random.Next(1,130);
                checkEmail = $"{prefixemail}{randomNumber}@{postfixemail}";
                var isExist=  context.users.SingleOrDefault(e => e.Email == checkEmail);            
                if(isExist==null||!SuggestionEmail.Contains(checkEmail)) 
                { 
                SuggestionEmail.Add(checkEmail);    
                } 
            }    
            return SuggestionEmail;
        }
    }
}
