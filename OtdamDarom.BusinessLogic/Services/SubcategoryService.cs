using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Services
{
    public class SubcategoryService : AdminApi, ISubcategory
    {
        public new async Task<SubcategoryModel> GetSubcategoryByIdAsync(int id)
        {
            return await base.GetSubcategoryByIdAsync(id);
        }

        public new async Task<IEnumerable<SubcategoryModel>> GetAllSubcategoriesAsync()
        {
            return await base.GetAllSubcategoriesAsync();
        }

        public new async Task<int> CreateSubcategoryAsync(SubcategoryModel newSubcategory)
        {
            return await base.CreateSubcategoryAsync(newSubcategory);
        }

        public new async Task UpdateSubcategoryAsync(SubcategoryModel subcategory)
        {
            await base.UpdateSubcategoryAsync(subcategory);
        }

        public new async Task DeleteSubcategoryAsync(int id)
        {
            await base.DeleteSubcategoryAsync(id);
        }
    }
}