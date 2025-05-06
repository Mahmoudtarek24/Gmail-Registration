namespace GmailRegistrationWeb.Core.Models
{
    [Index(nameof(Email),IsUnique =true)]
    [Index(nameof(PhoneNumber),IsUnique =true)]  
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int UsersId { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FirstName { get; set; } = null!;
		public string LastName { get; set; }= null!;    
		public DateTime DateOfBirth { get; set; }   
        public Gender gender { get; set; }  
        public string CountryCode { get; set; }= null!; 
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string? RecoveryEmail { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
