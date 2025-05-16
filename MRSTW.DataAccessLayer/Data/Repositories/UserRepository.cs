using Domain.Models.Auth;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly DbSet<AppUser> _dbSet = context.Set<AppUser>();

    public async Task<AppUser?> GetByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<List<AppUser>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(AppUser entity)
    {
        _dbSet.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AppUser entity)
    {
        var user = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);

        if (user != null)
        {
            user.Name = entity.Name;
            user.Email = entity.Email;
            user.Surname = entity.Surname;
        }
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AppUser entity)
    {
        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }
}
