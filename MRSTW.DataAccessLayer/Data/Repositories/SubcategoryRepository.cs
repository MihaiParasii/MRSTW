using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class SubcategoryRepository(AppDbContext context)
    : GenericRepository<SubcategoryModel>(context), ISubcategoryRepository
{
    private readonly AppDbContext _context = context;

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
        return await _context.Subcategories.Where(s => s.CategoryId == categoryId).AsNoTracking().ToListAsync();
    }
}
