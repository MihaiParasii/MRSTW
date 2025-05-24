using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Services
{
    public class DealService: UserApi,  IDeal
    {
        public new async Task<DealModel> GetDealByIdAsync(int id)
        {
            return await base.GetDealByIdAsync(id);
        }

        public new async Task<IEnumerable<DealModel>> GetAllDealsAsync()
        {
            return await base.GetAllDealsAsync();
        }

        public new async Task<int> CreateDealAsync(DealModel newDeal)
        {
            return await base.CreateDealAsync(newDeal);
        }

        public new async Task UpdateDealAsync(DealModel newDeal)
        {
            await base.UpdateDealAsync(newDeal);
        }

        public new async Task DeleteDealAsync(int id)
        {
            await base.DeleteDealAsync(id);
        }
    }
}