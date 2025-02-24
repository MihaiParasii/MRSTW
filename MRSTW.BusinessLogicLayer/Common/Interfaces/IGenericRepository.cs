using Domain.Common;
using MRSTW.BusinessLogicLayer.Common.Models;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<PaginatedList<T>> GetPaginatedListAsync(int pageSize, int pageCount);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
