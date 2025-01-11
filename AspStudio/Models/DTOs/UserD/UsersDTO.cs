using System.Diagnostics.Metrics;

namespace AspStudio.Models.DTOs.UserD
{
    public class UsersDTO
    {
        public int Id { get; set; }
        public IFormFile ImgFile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPassword { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
      
    }
}
