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
        public int SubcategoryId { get; set; } 
        public virtual SubcategoryModel Subcategory { get; set; } 
        
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
    }
}