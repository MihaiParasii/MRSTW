using System.ComponentModel.DataAnnotations;

namespace OtdamDarom.BusinessLogic.Dtos
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Adresa de email este obligatorie.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Formatul adresei de email este invalid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [Display(Name = "Parolă")]
        [DataType(DataType.Password)]
        // StringLength trebuie să corespundă cu ce ai în baza de date pentru input, nu pentru hash
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Parola trebuie să aibă între 6 și 100 de caractere.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } // Adăugat pentru funcția "Ține-mă minte"
    }
}