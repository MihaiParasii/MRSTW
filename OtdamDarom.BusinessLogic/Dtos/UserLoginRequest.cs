using System.ComponentModel.DataAnnotations;

namespace OtdamDarom.BusinessLogic.Dtos
{
    public class UserLoginRequest
    {
        [Required]
        [Display(Name = "Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
        
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}