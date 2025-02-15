namespace MRSTW.Api.Contracts.Deal;

public record DealResponse(
    int Id,
    string[] PhotoPaths,
    string Title,
    string Description,
    DateOnly CreationDate,
    string Subcategory);
