using Domain.Models;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.DataAccessLayer.Data.Repositories;

public class DealRepository(AppDbContext dbContext) : GenericRepository<Deal>(dbContext), IDealRepository
{
    
}
