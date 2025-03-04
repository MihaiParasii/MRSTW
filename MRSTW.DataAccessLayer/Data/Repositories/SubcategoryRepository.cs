using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class SubcategoryRepository()
    : GenericRepository<SubcategoryModel>(), ISubcategoryRepository
{
    public override async Task UpdateAsync(SubcategoryModel entity)
    {
        await DbSet
            .Where(e => e.Id == entity.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Name, entity.Name)
                .SetProperty(e => e.CategoryId, entity.CategoryId)
            );
    }

    public async Task<List<SubcategoryModel>> GetAllByCategoryIdAsync(int categoryId)
    {
        throw new NotSupportedException();
        // return await context.Subcategories.Where(s => s.CategoryId == categoryId).AsNoTracking().ToListAsync();
    }
}
