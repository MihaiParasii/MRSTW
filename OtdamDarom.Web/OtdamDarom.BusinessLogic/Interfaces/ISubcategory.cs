using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface ISubcategory
    {
        Task<SubcategoryModel> GetSubcategoryByIdAsync(int id);
        Task<IEnumerable<SubcategoryModel>> GetAllSubcategoriesAsync();
        Task<int> CreateSubcategoryAsync(SubcategoryModel newSubcategory);
        Task UpdateSubcategoryAsync(SubcategoryModel subcategory);
        Task DeleteSubcategoryAsync(int id);
    }
}