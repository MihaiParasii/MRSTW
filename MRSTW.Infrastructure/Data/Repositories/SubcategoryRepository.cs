using MRSTW.Application.Common.Interfaces;
using MRSTW.Domain.Models;

namespace MRSTW.Infrastructure.Data.Repositories;

public class SubcategoryRepository(AppDbContext dbContext)
    : GenericRepository<Subcategory>(dbContext), ISubcategoryRepository
{
}
