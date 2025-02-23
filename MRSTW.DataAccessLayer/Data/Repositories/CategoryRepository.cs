using Domain.Models;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class CategoryRepository(AppDbContext dbContext) : GenericRepository<Category>(dbContext), ICategoryRepository
{
    
}
