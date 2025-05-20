using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class CategoryRepository(AppDbContext context) : GenericRepository<CategoryModel>(context), ICategoryRepository
{
    public override async Task UpdateAsync(CategoryModel entity)
    {
        await DbSet.Where(e => e.Id == entity.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Name, entity.Name)
            );
    }

    public override async Task<List<CategoryModel>> GetAllAsync()
    {
        return await DbSet.Include(x => x.Subcategories).AsNoTracking().ToListAsync();
    }

    override public async Task<CategoryModel> GetByIdAsync(int id)
    {
        return await DbSet.Include(x => x.Subcategories).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}
