using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Services
{
    public class CategoryService : AdminApi,  ICategory
    {
        public new async Task<CategoryModel> GetCategoryByIdAsync(int id)
        {
            return await base.GetCategoryByIdAsync(id);
        }

        public new async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync()
        {
            return await base.GetAllCategoriesAsync();
        }

        public new async Task<int> CreateCategoryAsync(CategoryModel category)
        {
            return await base.CreateCategoryAsync(category);
        }

        public new async Task UpdateCategoryAsync(CategoryModel newCategory)
        {
            await base.UpdateCategoryAsync(newCategory);
        }

        public new async Task DeleteCategoryAsync(int id)
        {
            await base.DeleteCategoryAsync(id);
        }
    }
}