using Domain.Models.Main;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface ISubcategoryRepository : IGenericRepository<SubcategoryModel>
{
    Task<List<SubcategoryModel>> GetAllByCategoryIdAsync(int categoryId);
}
