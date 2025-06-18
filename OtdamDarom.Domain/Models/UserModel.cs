using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

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
        
        [StringLength(500)] 
        [Display(Name = "Imagine de Profil URL")]
        public string ProfilePictureUrl { get; set; } 

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

        public virtual ICollection<UserSession> Sessions { get; set; }

        // <<<<<<<<<<<<<<<<< ADAUGĂ ACEASTĂ LINIE >>>>>>>>>>>>>>>>>>>>>>
        public virtual ICollection<DealModel> Deals { get; set; } // Un utilizator poate avea mai multe anunțuri
        // <<<<<<<<<<<<<<<<< SFÂRȘIT ADĂUGARE >>>>>>>>>>>>>>>>>>>>>>

        public UserModel()
        {
            // <<<<<<<<<<<<<<<<< INIȚIALIZEAZĂ ȘI ACEASTĂ COLECȚIE >>>>>>>>>>>>>>>>>>>>>>
            Sessions = new HashSet<UserSession>();
            Deals = new HashSet<DealModel>(); // Asigură-te că este inițializată
            // <<<<<<<<<<<<<<<<< SFÂRȘIT INIȚIALIZARE >>>>>>>>>>>>>>>>>>>>>>
        }
    }
}