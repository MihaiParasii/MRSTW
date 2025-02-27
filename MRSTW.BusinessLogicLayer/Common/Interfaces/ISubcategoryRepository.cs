
using Domain.Models;
using Domain.Models.Main;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface ISubcategoryRepository : IGenericRepository<Subcategory>
{
    Task<List<Subcategory>> GetAllByCategoryIdAsync(int categoryId);
}
