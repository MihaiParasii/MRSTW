using Microsoft.EntityFrameworkCore;
using MRSTW.Application.Common.Interfaces;
using MRSTW.Application.Common.Mappings;
using MRSTW.Application.Common.Models;
using MRSTW.Domain.Common;

namespace MRSTW.Infrastructure.Data.Repositories;

public class GenericRepository<T>(AppDbContext dbContext) : IGenericRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaginatedList<T>> GetPaginatedListAsync(int pageSize, int pageNumber)
    {
        return await _dbSet.ToPaginatedListAsync(pageNumber, pageSize);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        _dbSet.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
