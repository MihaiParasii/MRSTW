using AutoMapper;
using Domain.Models;
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

    public void UpdateAsync(UpdateSubcategoryRequest request)
    {
        var subcategory = mapper.Map<Subcategory>(request);
        // Perform any necessary validation here
        // if (!IsValid(request.Deal))
        // {
        //     throw new ArgumentException("Invalid deal data.");
        // }


        subcategoryRepository.UpdateAsync(subcategory);
    }

    public async Task DeleteAsync(int id)
    {
        var subcategory = await subcategoryRepository.GetByIdAsync(id);

        if (subcategory == null)
        {
            throw new InvalidOperationException("Subcategory not found.");
        }

        await subcategoryRepository.DeleteAsync(subcategory);
    }

    public async Task<PaginatedList<SubcategoryResponse>> GetPaginatedListAsync(int pageSize, int pageCount)
    {
        var subcategories = await subcategoryRepository.GetPaginatedListAsync(pageSize, pageCount);

        return mapper.Map<PaginatedList<SubcategoryResponse>>(subcategories);
    }

    public async Task<SubcategoryResponse> GetByIdAsync(int id)
    {
        var subcategory = await subcategoryRepository.GetByIdAsync(id);

        if (subcategory == null)
        {
            throw new InvalidOperationException("Subcategory not found.");
        }

        return mapper.Map<SubcategoryResponse>(subcategory);
    }
}
