using Domain.Models;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class SubcategoryRepository(AppDbContext dbContext)
    : GenericRepository<Subcategory>(dbContext), ISubcategoryRepository
{
    public override async Task UpdateAsync(Subcategory entity)
    {
        await DbSet
            .Where(e => e.Id == entity.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Name, entity.Name)
                .SetProperty(e => e.CategoryId, entity.CategoryId)
            );
    }
}
