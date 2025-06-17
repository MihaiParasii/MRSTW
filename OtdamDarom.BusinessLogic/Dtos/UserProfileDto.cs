using System;
using System.ComponentModel.DataAnnotations;
using System.Web; // Adaugă acest using pentru HttpPostedFileBase

namespace OtdamDarom.BusinessLogic.Dtos
{
    public class UserProfileDto
    {
        public int Id { get; set; }

        [Display(Name = "Nume complet")]
        [Required(ErrorMessage = "Numele este obligatoriu.")]
        public string Name { get; set; }

        [Display(Name = "Adresă de email")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă.")]
        [Required(ErrorMessage = "Emailul este obligatoriu.")]
        public string Email { get; set; }

        [Display(Name = "Rol utilizator")]
        public string UserRole { get; set; }

        [Display(Name = "Imagine de profil")]
        public string ProfilePictureUrl { get; set; } 

        [Display(Name = "Selectează o imagine nouă")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase NewProfilePicture { get; set; } 

        [Display(Name = "Data înregistrării")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        // --- Câmpuri adăugate pentru schimbarea parolei ---
        [DataType(DataType.Password)]
        [Display(Name = "Parola actuală")]
        // Nu este Required aici, deoarece editarea profilului nu implică întotdeauna schimbarea parolei.
        // Validarea se va face în controller dacă aceste câmpuri sunt completate.
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Parola nouă")]
        [StringLength(100, ErrorMessage = "Parola nouă trebuie să aibă cel puțin {2} caractere.", MinimumLength = 6)]
        // Nu este Required aici, doar dacă CurrentPassword este completat.
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmă parola nouă")]
        [Compare("NewPassword", ErrorMessage = "Parola nouă și confirmarea parolei nu se potrivesc.")]
        // Nu este Required aici, doar dacă CurrentPassword este completat.
        public string ConfirmNewPassword { get; set; }
    }
}