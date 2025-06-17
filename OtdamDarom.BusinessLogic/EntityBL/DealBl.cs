using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api; // Asigură-te că UserApi este definit aici
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models; // Pentru DealModel

namespace OtdamDarom.BusinessLogic.EntityBL
{
    // Asigură-te că DealBl extinde UserApi și implementează IDeal
    public class DealBl : UserApi, IDeal
    {
        // Constructorul implicit este deja preluat de la UserApi

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

        // <<<<<<<<<<<<<<<<< ADAUGĂ/CORECTEAZĂ ACEASTĂ IMPLEMENTARE >>>>>>>>>>>>>>>>>>>>>>
        // Rezolvă: "DealBl" не реализует член интерфейса "IDeal.GetDealsBySubcategoryIds(List<int>)".
        public async Task<IEnumerable<DealModel>> GetDealsBySubcategoryIds(List<int> subcategoryIds)
        {
            return await GetDealsBySubcategoryIdsAsync(subcategoryIds); 
        }
        // <<<<<<<<<<<<<<<<< SFÂRȘIT CORECTARE >>>>>>>>>>>>>>>>>>>>>>

        // <<<<<<<<<<<<<<<<< ADAUGĂ ACEASTĂ IMPLEMENTARE >>>>>>>>>>>>>>>>>>>>>>
        // Rezolvă: "DealBl" не реализует член интерфейса "IDeal.SearchDeals(string)".
        public async Task<IEnumerable<DealModel>> SearchDeals(string query)
        {
            return await SearchDealsAsync(query);
        }
        // <<<<<<<<<<<<<<<<< SFÂRȘIT ADAUGĂ >>>>>>>>>>>>>>>>>>>>>>
    }
}