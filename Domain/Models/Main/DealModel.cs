using Domain.Common;

namespace Domain.Models.Main;

public class DealModel : BaseEntity
{
    public required Guid UserId { get; set; }
    public required string[] PhotoPaths { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }

    public required DateOnly CreationDate { get; set; }

    public required int SubcategoryId { get; set; }
    public required SubcategoryModel SubcategoryModel { get; set; }
}
