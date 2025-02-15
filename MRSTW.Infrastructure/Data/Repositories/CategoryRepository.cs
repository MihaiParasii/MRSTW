using MRSTW.Application.Common.Interfaces;
using MRSTW.Domain.Models;

namespace MRSTW.Infrastructure.Data.Repositories;

public class CategoryRepository(AppDbContext dbContext) : GenericRepository<Category>(dbContext), ICategoryRepository
{
    
}
