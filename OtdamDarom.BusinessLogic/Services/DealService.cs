using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Services
{
    public class DealService : IDeal
    {
        private readonly IDeal _dealBl;

        public DealService() 
        {
            _dealBl = new OtdamDarom.BusinessLogic.EntityBL.DealBl(); 
        }

        public async Task<DealModel> GetById(int id)
        {
            return await _dealBl.GetById(id);
        }

        public async Task<IEnumerable<DealModel>> GetAll()
        {
            return await _dealBl.GetAll();
        }

        public async Task Create(DealModel dealData)
        {
            await _dealBl.Create(dealData);
        }

        public async Task Update(DealModel dealData)
        {
            await _dealBl.Update(dealData);
        }

        public async Task Delete(int dealId)
        {
            await _dealBl.Delete(dealId);
        }

        public async Task<IEnumerable<DealModel>> GetDealsByCategoryId(int categoryId)
        {
            return await _dealBl.GetDealsByCategoryId(categoryId);
        }

        public async Task<IEnumerable<DealModel>> GetDealsBySubcategoryIds(List<int> subcategoryIds)
        {
            return await _dealBl.GetDealsBySubcategoryIds(subcategoryIds);
        }

        public async Task<IEnumerable<DealModel>> SearchDeals(string query)
        {
            return await _dealBl.SearchDeals(query);
        }
        
        public async Task<IEnumerable<DealModel>> GetDealsByUserId(int userId)
        {
            return await _dealBl.GetDealsByUserId(userId);
        }
    }
}