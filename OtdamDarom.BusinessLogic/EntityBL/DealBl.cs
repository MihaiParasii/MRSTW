using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.EntityBL
{
    public class DealBl : UserApi, IDeal
    {
        public async Task<DealModel> GetById(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<IEnumerable<DealModel>> GetAll()
        {
            return await GetAllAsync();
        }
        
        public async Task Create(DealModel dealData)
        {
            await CreateDealAsync(dealData);
        }

        public async Task Update(DealModel dealData)
        {
            await UpdateDealAsync(dealData);
        }

        public async Task Delete(int dealId)
        {
            await DeleteDealAsync(dealId);
        }
        
        public async Task<IEnumerable<DealModel>> GetDealsByCategoryId(int categoryId)
        {
            return await GetDealsByCategoryIdAsync(categoryId);
        }
    }
}