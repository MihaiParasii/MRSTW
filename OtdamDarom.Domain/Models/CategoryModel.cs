using System.Collections.Generic;

namespace OtdamDarom.Domain.Models
{
    public class CategoryModel : BaseEntity
    {
        public string Name { get; set; }
        public List<int> SubcategoryIds { get; set; } = new List<int>();
        public List<SubcategoryModel> Subcategories { get; set; } = new List<SubcategoryModel>();
    }
}