using Domain.Models;
using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository
{
    public override async Task UpdateAsync(Category entity)
    {
        await DbSet.Where(e => e.Id == entity.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Name, entity.Name)
            );
    }
}
