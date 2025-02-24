using Domain.Models;
using MRSTW.BusinessLogicLayer.Common.Models;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface IDealRepository : IGenericRepository<Deal>
{
    Task<PaginatedList<Deal>> GetPaginatedListAsync(int pageSize, int pageCount);
}
