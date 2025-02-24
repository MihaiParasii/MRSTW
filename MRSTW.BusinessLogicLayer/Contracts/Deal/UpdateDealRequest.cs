namespace MRSTW.BusinessLogicLayer.Contracts.Deal;

public record UpdateDealRequest(
    int Id,
    string Title,
    string Description,
    string[] PhotoPaths);
