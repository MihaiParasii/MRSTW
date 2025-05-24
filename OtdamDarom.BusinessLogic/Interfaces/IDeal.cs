using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface IDeal
    {
        Task<DealModel> GetDealByIdAsync(int id);
        Task<IEnumerable<DealModel>> GetAllDealsAsync();
        Task<int> CreateDealAsync(DealModel newDeal);
        Task UpdateDealAsync(DealModel newDeal);
        Task DeleteDealAsync(int id);
    }
}