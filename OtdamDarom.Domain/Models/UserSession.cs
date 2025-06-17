using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtdamDarom.Domain.Models
{
    [Table("UserSessions")]
    public class UserSession
    {
        [Key]
        public int Id { get; set; }

        [Required] // UserId este obligatoriu
        public int UserId { get; set; }

        [Required]
        [StringLength(255)] // Lungime potrivită pentru GUID
        public string Token { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        // Proprietate de navigare către User
        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
    }
}