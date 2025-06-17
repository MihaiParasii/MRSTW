using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic; // Adaugă acest using pentru ICollection

namespace OtdamDarom.Domain.Models
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)] 
        [Display(Name = "Nume")] 
        public string Name { get; set; }
        
        // Proprietate pentru URL-ul imaginii de profil - ACEASTA ESTE NOUA SI ESTE FOARTE BINE CA AI ADAUGAT-O!
        [StringLength(500)] 
        [Display(Name = "Imagine de Profil URL")]
        public string ProfilePictureUrl { get; set; } // Poate fi null dacă utilizatorul nu are o imagine setată

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)] 
        public string Email { get; set; }

        [Required]
        [Display(Name = "Parolă Hash")] 
        [DataType(DataType.Password)]
        [StringLength(255)] 
        public string PasswordHash { get; set; }

        [Required] 
        [StringLength(50)] 
        [Display(Name = "Rol Utilizator")]
        public string UserRole { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow; 

        // Proprietate de navigare pentru sesiuni (opțional, dar bun pentru EF)
        public virtual ICollection<UserSession> Sessions { get; set; }
    }
}