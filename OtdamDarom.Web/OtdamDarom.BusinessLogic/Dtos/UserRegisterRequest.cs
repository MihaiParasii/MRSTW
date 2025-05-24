using System.ComponentModel.DataAnnotations;

namespace OtdamDarom.BusinessLogic.Dtos
{
    public class UserRegisterRequest
    {
        [Required] [Display(Name = "Name")] 
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
        
        [Required]
        [Display(Name = "User Role")]
        public string UserRole { get; set; }
    }
}