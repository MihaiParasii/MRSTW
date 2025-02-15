using MRSTW.Application.Common.Interfaces;
using MRSTW.Domain.Models;

namespace MRSTW.Infrastructure.Data.Repositories;

public class DealRepository(AppDbContext dbContext) : GenericRepository<Deal>(dbContext), IDealRepository
{
    
}
