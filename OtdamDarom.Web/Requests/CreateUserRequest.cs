using System.ComponentModel.DataAnnotations;

namespace OtdamDarom.Web.Requests
{
    public class CreateUserRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string PasswordHash { get; set; }

        public string UserRole { get; set; }
    }
}