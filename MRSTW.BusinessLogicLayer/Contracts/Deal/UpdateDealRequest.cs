namespace MRSTW.BusinessLogicLayer.Contracts.Deal;

public record UpdateDealRequest(
    string Id,
    string Title,
    string Description,
    string[] PhotoPaths);
