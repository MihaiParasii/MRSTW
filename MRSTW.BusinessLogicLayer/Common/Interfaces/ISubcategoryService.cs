using MRSTW.BusinessLogicLayer.Contracts.Subcategory;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface ISubcategoryService
{
    Task CreateAsync(CreateSubcategoryRequest request);
    Task UpdateAsync(UpdateSubcategoryRequest request);
    Task DeleteAsync(int id);
    Task<SubcategoryResponse> GetByIdAsync(int id);
    Task<List<SubcategoryResponse>> GetAllAsync();
}
