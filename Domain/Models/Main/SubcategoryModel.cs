using Domain.Common;

namespace Domain.Models.Main;

public class SubcategoryModel : BaseEntity
{
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
    public required CategoryModel CategoryModel { get; set; }
    public List<DealModel> Deals { get; set; } = [];
}
