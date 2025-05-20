namespace MRSTW.BusinessLogicLayer.Contracts.Deal;

public class UpdateDealRequest
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int SubcategoryId { get; init; }
    public string[] PhotoPaths { get; init; }
}
