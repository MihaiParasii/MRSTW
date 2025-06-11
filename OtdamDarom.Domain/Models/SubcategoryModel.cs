using System.ComponentModel.DataAnnotations.Schema;

namespace OtdamDarom.Domain.Models
{
    [Table("Subcategories")]
    public class SubcategoryModel : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryModel Category { get; set; }
        
    }
}