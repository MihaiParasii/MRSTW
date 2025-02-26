using AutoMapper;
using Domain.Models;
using Domain.Models.Main;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Common.Models;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;

namespace MRSTW.BusinessLogicLayer.Services;

public class SubcategoryService(ISubcategoryRepository subcategoryRepository, IMapper mapper)
{
    public async Task CreateAsync(CreateSubcategoryRequest request)
    {
        var deal = mapper.Map<Subcategory>(request);


        // Perform any necessary validation here
        // if (!IsValid(deal))
        // {
        //     throw new ArgumentException("Invalid deal data.");
        // }


        await subcategoryRepository.AddAsync(deal);
    }

    public async Task UpdateAsync(UpdateSubcategoryRequest request)
    {
        var subcategory = mapper.Map<Subcategory>(request);
        // Perform any necessary validation here
        // if (!IsValid(request.Deal))
        // {
        //     throw new ArgumentException("Invalid deal data.");
        // }


        await subcategoryRepository.UpdateAsync(subcategory);
    }

    public async Task DeleteAsync(int id)
    {
        var subcategory = await subcategoryRepository.GetByIdAsync(id);

        if (subcategory == null)
        {
            throw new ArgumentException("Subcategory not found.");
        }

        await subcategoryRepository.DeleteAsync(subcategory);
    }

    public async Task<SubcategoryResponse> GetByIdAsync(int id)
    {
        var subcategory = await subcategoryRepository.GetByIdAsync(id);

        if (subcategory == null)
        {
            throw new ArgumentException("Subcategory not found.");
        }

        return mapper.Map<SubcategoryResponse>(subcategory);
    }

    public async Task<List<SubcategoryResponse>> GetAllAsync()
    {
        var subcategories = await subcategoryRepository.GetAllAsync();
        return mapper.Map<List<SubcategoryResponse>>(subcategories);
    }
}
