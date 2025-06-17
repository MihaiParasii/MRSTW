// OtdamDarom.BusinessLogic.Services/DealService.cs (sau calea corectă a fișierului)
using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Interfaces; // Asigură-te că IDeal este importat
using OtdamDarom.Domain.Models; // Asigură-te că DealModel este importat

namespace OtdamDarom.BusinessLogic.Services // Sau namespace-ul corect
{
    // Presupun că DealService folosește o instanță de IDeal sau un alt API
    // Voi adăuga o instanță simplă a DealBl pentru exemplificare,
    // dar ar trebui să folosești mecanismul tău de injectare a dependențelor.
    public class DealService : IDeal
    {
        private readonly IDeal _dealBl; // Sau un alt tip, depinde cum e structura ta

        public DealService() 
        {
             // Aceasta este doar o modalitate simplă de a inițializa pentru a compila.
             // În aplicații reale, se face prin Dependency Injection.
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

        // <<<<<<<<<<<<<<<<< ADAUGĂ ACEASTĂ IMPLEMENTARE >>>>>>>>>>>>>>>>>>>>>>
        // Rezolvă: "DealService" не реализует член интерфейса "IDeal.SearchDeals(string)".
        public async Task<IEnumerable<DealModel>> SearchDeals(string query)
        {
            return await _dealBl.SearchDeals(query);
        }
        // <<<<<<<<<<<<<<<<< SFÂRȘIT ADAUGĂ >>>>>>>>>>>>>>>>>>>>>>
    }
}