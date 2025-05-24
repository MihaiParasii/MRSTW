namespace OtdamDarom.Domain.Models
{
    public class SubcategoryModel : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
    }
}