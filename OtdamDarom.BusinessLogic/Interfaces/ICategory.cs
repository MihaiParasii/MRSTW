using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface ICategory
    {
        Task<CategoryModel> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync();   
        Task<int> CreateCategoryAsync(CategoryModel category);
        Task UpdateCategoryAsync(CategoryModel newCategory);
        Task DeleteCategoryAsync(int id);
    }
}