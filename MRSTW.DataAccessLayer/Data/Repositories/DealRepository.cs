using Domain.Models;
using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Common.Mappings;
using MRSTW.BusinessLogicLayer.Common.Models;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class DealRepository(AppDbContext dbContext) : GenericRepository<Deal>(dbContext), IDealRepository
{
    public override async Task UpdateAsync(Deal entity)
    {
        await DbSet
            .Where(e => e.Id == entity.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Title, entity.Title)
                .SetProperty(e => e.Description, entity.Description)
                .SetProperty(e => e.SubcategoryId, entity.SubcategoryId)
            );
    }
    
    public async Task<PaginatedList<Deal>> GetPaginatedListAsync(int pageSize, int pageNumber)
    {
        return await DbSet.ToPaginatedListAsync(pageNumber, pageSize);
    }
}
