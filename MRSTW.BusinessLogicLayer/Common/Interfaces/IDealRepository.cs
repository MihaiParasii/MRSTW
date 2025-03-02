using Domain.Models.Main;
using MRSTW.BusinessLogicLayer.Common.Models;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface IDealRepository : IGenericRepository<DealModel>
{
    Task<PaginatedList<DealModel>> GetPaginatedListAsync(int pageSize, int pageCount);
}
