using Domain.Models;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class SubcategoryRepository(AppDbContext dbContext)
    : GenericRepository<Subcategory>(dbContext), ISubcategoryRepository
{
}
