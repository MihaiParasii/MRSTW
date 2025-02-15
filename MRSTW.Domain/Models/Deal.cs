using MRSTW.Domain.Common;

namespace MRSTW.Domain.Models;

public class Deal : BaseEntity
{
    public required Guid UserId { get; set; }
    public required string[] PhotoPaths { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }

    public required DateOnly CreationDate { get; set; }

    public required int SubcategoryId { get; set; }
    public required Subcategory Subcategory { get; set; }
}
