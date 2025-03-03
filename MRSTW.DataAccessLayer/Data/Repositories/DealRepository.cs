using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Common.Mappings;
using MRSTW.BusinessLogicLayer.Common.Models;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class DealRepository(AppDbContext context) : GenericRepository<DealModel>(context), IDealRepository
{
    public override async Task UpdateAsync(DealModel entity)
    {
        await DbSet
            .Where(e => e.Id == entity.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.Title, entity.Title)
                .SetProperty(e => e.Description, entity.Description)
                .SetProperty(e => e.SubcategoryId, entity.SubcategoryId)
            );
    }

    public async Task<PaginatedList<DealModel>> GetPaginatedListAsync(int pageSize, int pageNumber)
    {
        return await DbSet.ToPaginatedListAsync(pageNumber, pageSize);
    }
}
