namespace MRSTW.BusinessLogicLayer.Contracts.Subcategory;

public class UpdateSubcategoryRequest
{
    public int Id { get; init; }
    public string Name { get; init; }
    public int CategoryId { get; init; }
}
