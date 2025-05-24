using System;
using System.ComponentModel.DataAnnotations;

namespace OtdamDarom.Domain.Models
{
    public class UserSession
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }   
    }
}