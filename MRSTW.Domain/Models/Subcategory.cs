using MRSTW.Domain.Common;

namespace MRSTW.Domain.Models;

public class Subcategory : BaseEntity
{
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
    public required Category Category { get; set; }
    public List<Deal> Deals { get; set; } = [];
}
