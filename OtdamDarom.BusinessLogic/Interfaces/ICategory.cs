using System.Collections.Generic;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface ICategory
    {
        IEnumerable<CategoryModel> GetAllCategoriesWithSubcategories();
    }
}