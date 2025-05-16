using Domain.Common;
using Domain.Models.Auth;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

public interface IUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email);
    Task<List<AppUser>> GetAllAsync();
    Task AddAsync(AppUser entity);
    Task UpdateAsync(AppUser entity);
    Task DeleteAsync(AppUser entity);
}
