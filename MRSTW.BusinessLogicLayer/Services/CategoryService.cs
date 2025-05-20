using AutoMapper;
using Domain.Models.Main;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Contracts.Category;

namespace MRSTW.BusinessLogicLayer.Services;

public class CategoryService(IBusinessUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
{
    public async Task CreateAsync(CreateCategoryRequest request)
    {
        var category = unitOfWork.Mapper.Map<CategoryModel>(request);

        var validationResult = await unitOfWork.CategoryValidator.ValidateAsync(category);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors[0].ToString());
        }

        await unitOfWork.CategoryRepository.AddAsync(category);
    }

    public async Task UpdateAsync(UpdateCategoryRequest request)
    {
        var category = unitOfWork.Mapper.Map<CategoryModel>(request);

        var validationResult = await unitOfWork.CategoryValidator.ValidateAsync(category);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors[0].ToString());
        }

        await unitOfWork.CategoryRepository.UpdateAsync(category);
    }

    public async Task DeleteAsync(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("Invalid category ID.");
        }

        var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            throw new ArgumentException("Category not found.");
        }

        await unitOfWork.CategoryRepository.DeleteAsync(category);
    }

    public async Task<CategoryResponse> GetByIdAsync(int id)
    {
        var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            throw new ArgumentException("Category not found.");
        }

        return unitOfWork.Mapper.Map<CategoryResponse>(category);
    }

    public async Task<List<CategoryResponse>> GetAllAsync()
    {
        var categories = await unitOfWork.CategoryRepository.GetAllAsync();

        List<CategoryResponse> result = [];

        foreach (var category in categories)
        {
            result.Add(unitOfWork.Mapper.Map<CategoryResponse>(category));
        }
        
        return result;
    }
}
