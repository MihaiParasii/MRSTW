using AutoMapper;
using Domain.Models.Main;
using FluentValidation;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Contracts.Category;

namespace MRSTW.BusinessLogicLayer.Services;

public class CategoryService(
    ICategoryRepository categoryRepository,
    IMapper mapper,
    IValidator<CategoryModel> categoryValidator)
{
    public async Task CreateAsync(CreateCategoryRequest request)
    {
        var category = mapper.Map<CategoryModel>(request);

        var validationResult = await categoryValidator.ValidateAsync(category);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Invalid category data.");
        }

        await categoryRepository.AddAsync(category);
    }

    public async Task UpdateAsync(UpdateCategoryRequest request)
    {
        var category = mapper.Map<CategoryModel>(request);

        var validationResult = await categoryValidator.ValidateAsync(category);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Invalid category data.");
        }

        await categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteAsync(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("Invalid category ID.");
        }
        
        var category = await categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            throw new ArgumentException("Category not found.");
        }

        await categoryRepository.DeleteAsync(category);
    }

    public async Task<CategoryResponse> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            throw new ArgumentException("Category not found.");
        }

        return mapper.Map<CategoryResponse>(category);
    }

    public async Task<List<CategoryResponse>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllAsync();

        var result = mapper.Map<List<CategoryResponse>>(categories);

        return result;
    }
}
