using Domain.Common;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public abstract class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        DbSet.Add(entity);
        await context.SaveChangesAsync();
    }

    public abstract Task UpdateAsync(T entity);

    public virtual async Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        await context.SaveChangesAsync();
    }
}