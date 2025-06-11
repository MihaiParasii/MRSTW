using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtdamDarom.Domain.Models
{
    [Table("Categories")]
    public class CategoryModel : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<SubcategoryModel> Subcategories { get; set; } = new List<SubcategoryModel>();
    }
}