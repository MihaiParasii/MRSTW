namespace MRSTW.BusinessLogicLayer.Contracts.Deal;

public record CreateDealRequest(
    string[] PhotoPaths,
    string Title,
    string Description,
    int SubcategoryId);
