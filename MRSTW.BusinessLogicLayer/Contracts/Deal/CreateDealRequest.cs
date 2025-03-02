namespace MRSTW.BusinessLogicLayer.Contracts.Deal;

public record CreateDealRequest(
    string Title,
    string Description,
    int SubcategoryId);
