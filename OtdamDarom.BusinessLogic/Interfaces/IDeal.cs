using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface IDeal
    {
        Task<DealModel> GetById(int id);
        Task<IEnumerable<DealModel>> GetAll();
        Task Create(DealModel dealData);
        Task Update(DealModel dealData);
        Task Delete(int dealId);
        
        Task<IEnumerable<DealModel>> GetDealsByCategoryId(int categoryId); 
        Task<IEnumerable<DealModel>> GetDealsBySubcategoryIds(List<int> subcategoryIds);
        Task<IEnumerable<DealModel>> SearchDeals(string query);
        Task<IEnumerable<DealModel>> GetDealsByUserId(int userId);
    }
}