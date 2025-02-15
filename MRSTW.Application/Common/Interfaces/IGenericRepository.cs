using MRSTW.Application.Common.Models;
using MRSTW.Domain.Common;

namespace MRSTW.Application.Common.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<PaginatedList<T>> GetPaginatedListAsync(int pageSize, int pageCount);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
