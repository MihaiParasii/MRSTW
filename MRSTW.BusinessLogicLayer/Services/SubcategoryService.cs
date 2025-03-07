using Domain.Models.Main;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;

namespace MRSTW.BusinessLogicLayer.Services;

public class SubcategoryService(IBusinessUnitOfWork unitOfWork) : ISubcategoryService
{
    public async Task CreateAsync(CreateSubcategoryRequest request)
    {
        var deal = unitOfWork.Mapper.Map<SubcategoryModel>(request);

        var validationResult = await unitOfWork.SubcategoryValidator.ValidateAsync(deal);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors[0].ToString());
        }

        await unitOfWork.SubcategoryRepository.AddAsync(deal);
    }

    public async Task UpdateAsync(UpdateSubcategoryRequest request)
    {
        var subcategory = unitOfWork.Mapper.Map<SubcategoryModel>(request);
        
        var validationResult = await unitOfWork.SubcategoryValidator.ValidateAsync(subcategory);
        
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors[0].ToString());
        }
        
        await unitOfWork.SubcategoryRepository.UpdateAsync(subcategory);
    }

    public async Task DeleteAsync(int id)
    {
        var subcategory = await unitOfWork.SubcategoryRepository.GetByIdAsync(id);

        if (subcategory == null)
        {
            throw new ArgumentException("Subcategory not found.");
        }

        await unitOfWork.SubcategoryRepository.DeleteAsync(subcategory);
    }

    public async Task<SubcategoryResponse> GetByIdAsync(int id)
    {
        var subcategory = await unitOfWork.SubcategoryRepository.GetByIdAsync(id);

        if (subcategory == null)
        {
            throw new ArgumentException("Subcategory not found.");
        }

        return unitOfWork.Mapper.Map<SubcategoryResponse>(subcategory);
    }

    public async Task<List<SubcategoryResponse>> GetAllAsync()
    {
        var subcategories = await unitOfWork.SubcategoryRepository.GetAllAsync();
        return unitOfWork.Mapper.Map<List<SubcategoryResponse>>(subcategories);
    }
}
