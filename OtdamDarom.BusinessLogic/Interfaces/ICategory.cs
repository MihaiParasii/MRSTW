using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface ICategory
    {
        IEnumerable<CategoryModel> GetAllCategoriesWithSubcategories();
        Task<CategoryModel> GetCategoryById(int id); 
    }
}