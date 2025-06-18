using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtdamDarom.Domain.Models
{
    [Table("Deals")]
    public class DealModel : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreationDate { get; set; }
        
        // <<<<<<<<<<<<<<<<< CORECTAT AICI >>>>>>>>>>>>>>>>>>>>>>
        public int? SubcategoryId { get; set; } // Fă-l nullable
        public virtual SubcategoryModel Subcategory { get; set; } 
        // <<<<<<<<<<<<<<<<< SFÂRȘIT CORECTAT >>>>>>>>>>>>>>>>>>>>>>
        
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
    }
}