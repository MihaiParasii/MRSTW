using AutoMapper;
using Domain.Models;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Common.Models;
using MRSTW.BusinessLogicLayer.Contracts.Category;

namespace MRSTW.BusinessLogicLayer.Services;

public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
{
    public async Task CreateAsync(CreateCategoryRequest request)
    {
        var category = mapper.Map<Category>(request);


        // Perform any necessary validation here
        // if (!IsValid(deal))
        // {
        //     throw new ArgumentException("Invalid deal data.");
        // }


        await categoryRepository.AddAsync(category);
    }

    public void UpdateAsync(UpdateCategoryRequest request)
    {
        var category = mapper.Map<Category>(request);
        // Perform any necessary validation here
        // if (!IsValid(request.Deal))
        // {
        //     throw new ArgumentException("Invalid deal data.");
        // }


        categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            throw new InvalidOperationException("Category not found.");
        }

        await categoryRepository.DeleteAsync(category);
    }

    public async Task<PaginatedList<CategoryResponse>> GetPaginatedListAsync(int pageSize, int pageCount)
    {
        var categories = await categoryRepository.GetPaginatedListAsync(pageSize, pageCount);

        return mapper.Map<PaginatedList<CategoryResponse>>(categories);
    }

    public async Task<CategoryResponse> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            throw new InvalidOperationException("Category not found.");
        }

        return mapper.Map<CategoryResponse>(category);
    }
}
