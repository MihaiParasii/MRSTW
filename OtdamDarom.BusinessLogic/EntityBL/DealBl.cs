using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.EntityBL
{
    public class DealBl : IDeal
    {
        private readonly UserApi _userApi;

        public DealBl()
        {
            _userApi = new UserApi();
        }

        public async Task<DealModel> GetById(int id)
        {
            return await _userApi.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DealModel>> GetAll()
        {
            return await _userApi.GetAllAsync();
        }
        
        public async Task Create(DealModel dealData)
        {
            await _userApi.CreateDealAsync(dealData);
        }

        public async Task Update(DealModel dealData)
        {
            await _userApi.UpdateDealAsync(dealData);
        }

        public async Task Delete(int dealId)
        {
            await _userApi.DeleteDealAsync(dealId);
        }
        
        public async Task<IEnumerable<DealModel>> GetDealsByCategoryId(int categoryId)
        {
            return await _userApi.GetDealsByCategoryIdAsync(categoryId);
        }

        public async Task<IEnumerable<DealModel>> GetDealsBySubcategoryIds(List<int> subcategoryIds)
        {
            return await _userApi.GetDealsBySubcategoryIdsAsync(subcategoryIds);
        }

        public async Task<IEnumerable<DealModel>> SearchDeals(string query)
        {
            return await _userApi.SearchDealsAsync(query);
        }
        public async Task<IEnumerable<DealModel>> GetDealsByUserId(int userId)
        {
            return await _userApi.GetDealsByUserIdAsync(userId);
        }
    }
}