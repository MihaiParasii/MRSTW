namespace MRSTW.BusinessLogicLayer.Contracts.Category;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<int> SubcategoryIds { get; set; } = [];
};
