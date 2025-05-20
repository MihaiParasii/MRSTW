namespace MRSTW.BusinessLogicLayer.Contracts.Deal;

public class DealResponse
{
    public int Id { get; init; }
    public string[] PhotoPaths { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateOnly CreationDate { get; init; }
    public string Subcategory { get; init; }
}

