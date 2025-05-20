namespace MRSTW.BusinessLogicLayer.Contracts.Deal;

public class CreateDealRequest
{
    public string Title { get; init; }
    public string Description { get; init; }
    public int SubcategoryId { get; init; }
}
