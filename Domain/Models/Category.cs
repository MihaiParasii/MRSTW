using Domain.Common;

namespace Domain.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<Subcategory> Subcategories { get; set; } = [];
}
