using System.ComponentModel.DataAnnotations;

namespace OtdamDarom.BusinessLogic.Dtos
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Numele este obligatoriu.")]
        [Display(Name = "Nume")]
        [StringLength(100, ErrorMessage = "Numele nu poate depăși 100 de caractere.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adresa de email este obligatorie.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Formatul adresei de email este invalid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [Display(Name = "Parolă")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Parola trebuie să aibă între 6 și 100 de caractere.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmă Parola")]
        [Compare("Password", ErrorMessage = "Parola de confirmare nu se potrivește.")]
        public string ConfirmPassword { get; set; } // Adăugat pentru confirmarea parolei

        [Required(ErrorMessage = "Rolul utilizatorului este obligatoriu.")]
        [Display(Name = "Rol Utilizator")]
        // Aici ar putea fi un DropDownList în UI, cu valori predefinite (User, Artist, Admin)
        public string UserRole { get; set; }
    }
}