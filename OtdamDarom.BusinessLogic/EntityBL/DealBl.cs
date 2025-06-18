using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api; // Asigură-te că UserApi este definit aici
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models; // Pentru DealModel

namespace OtdamDarom.BusinessLogic.EntityBL
{
    public class DealBl : IDeal // Nu mai extinde UserApi, ci folosește-l ca dependență
    {
        private readonly UserApi _userApi; // Instanță privată pentru a accesa metodele din UserApi

        public DealBl()
        {
            _userApi = new UserApi(); // Inițializează UserApi
        }

        public async Task<DealModel> GetById(int id)
        {
            return await _userApi.GetByIdAsync(id); // Apelează metoda din UserApi
        }

        public async Task<IEnumerable<DealModel>> GetAll()
        {
            return await _userApi.GetAllAsync(); // Apelează metoda din UserApi
        }
        
        public async Task Create(DealModel dealData)
        {
            await _userApi.CreateDealAsync(dealData); // Apelează metoda din UserApi
        }

        public async Task Update(DealModel dealData)
        {
            await _userApi.UpdateDealAsync(dealData); // Apelează metoda din UserApi
        }

        public async Task Delete(int dealId)
        {
            await _userApi.DeleteDealAsync(dealId); // Apelează metoda din UserApi
        }
        
        public async Task<IEnumerable<DealModel>> GetDealsByCategoryId(int categoryId)
        {
            return await _userApi.GetDealsByCategoryIdAsync(categoryId); // Apelează metoda din UserApi
        }

        public async Task<IEnumerable<DealModel>> GetDealsBySubcategoryIds(List<int> subcategoryIds)
        {
            return await _userApi.GetDealsBySubcategoryIdsAsync(subcategoryIds); // Apelează metoda din UserApi
        }

        public async Task<IEnumerable<DealModel>> SearchDeals(string query)
        {
            return await _userApi.SearchDealsAsync(query); // Apelează metoda din UserApi
        }

        // <<<<<<<<<<<<<<<<< NOU: ADAUGĂ ACEASTĂ IMPLEMENTARE >>>>>>>>>>>>>>>>>>>>>>
        public async Task<IEnumerable<DealModel>> GetDealsByUserId(int userId)
        {
            return await _userApi.GetDealsByUserIdAsync(userId); // Apelează metoda corespunzătoare din UserApi
        }
        // <<<<<<<<<<<<<<<<< SFÂRȘIT NOU >>>>>>>>>>>>>>>>>>>>>>
    }
}