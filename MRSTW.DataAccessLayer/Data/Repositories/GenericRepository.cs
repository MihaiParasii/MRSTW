using Domain.Common;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Common.Mappings;
using MRSTW.BusinessLogicLayer.Common.Models;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public abstract class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        DbSet.Add(entity);
        await context.SaveChangesAsync();
    }

    public abstract Task UpdateAsync(T entity);

    public async Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        await context.SaveChangesAsync();
    }
}
