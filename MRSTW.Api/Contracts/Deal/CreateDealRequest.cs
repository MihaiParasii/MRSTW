namespace MRSTW.Api.Contracts.Deal;

public record CreateDealRequest(
    string[] PhotoPaths,
    string Title,
    string Description,
    int SubcategoryId);
