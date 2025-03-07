using MRSTW.BusinessLogicLayer.Contracts.Category;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface ICategoryService
{
    Task CreateAsync(CreateCategoryRequest request);
    Task UpdateAsync(UpdateCategoryRequest request);
    Task DeleteAsync(int id);
    Task<CategoryResponse> GetByIdAsync(int id);
    Task<List<CategoryResponse>> GetAllAsync();
}
