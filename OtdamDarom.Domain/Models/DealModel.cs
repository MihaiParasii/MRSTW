using System;

namespace OtdamDarom.Domain.Models
{
    public class DealModel : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int SubcategoryId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}