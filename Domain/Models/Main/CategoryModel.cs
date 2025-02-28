using Domain.Common;

namespace Domain.Models.Main;

public class CategoryModel : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<SubcategoryModel> Subcategories { get; set; } = [];
}
