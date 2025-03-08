using MRSTW.BusinessLogicLayer.Common.Models;
using MRSTW.BusinessLogicLayer.Contracts.Deal;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface IDealService
{
    Task CreateAsync(CreateDealRequest request, List<string> filePaths);
    Task UpdateAsync(UpdateDealRequest request, List<string> filePaths);
    Task DeleteAsync(int id);
    Task<PaginatedList<DealResponse>> GetPaginatedListAsync(int pageSize, int pageCount);
    Task<DealResponse> GetByIdAsync(int id);
}
