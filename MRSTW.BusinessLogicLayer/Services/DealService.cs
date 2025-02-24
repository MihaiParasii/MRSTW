using AutoMapper;
using Domain.Models;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Common.Models;
using MRSTW.BusinessLogicLayer.Contracts.Deal;

namespace MRSTW.BusinessLogicLayer.Services;

public class DealService(IDealRepository dealRepository, IMapper mapper)
{
    public async Task CreateAsync(CreateDealRequest request)
    {
        var deal = mapper.Map<Deal>(request);


        // Perform any necessary validation here
        // if (!IsValid(deal))
        // {
        //     throw new ArgumentException("Invalid deal data.");
        // }


        await dealRepository.AddAsync(deal);
    }

    public async Task UpdateAsync(UpdateDealRequest request)
    {
        var deal = mapper.Map<Deal>(request);
        // Perform any necessary validation here
        // if (!IsValid(request.Deal))
        // {
        //     throw new ArgumentException("Invalid deal data.");
        // }


        await dealRepository.UpdateAsync(deal);
    }

    public async Task DeleteAsync(int id)
    {
        var deal = await dealRepository.GetByIdAsync(id);

        if (deal == null)
        {
            throw new InvalidOperationException("Deal not found.");
        }

        await dealRepository.DeleteAsync(deal);
    }

    public async Task<PaginatedList<DealResponse>> GetPaginatedListAsync(int pageSize, int pageCount)
    {
        var deals = await dealRepository.GetPaginatedListAsync(pageSize, pageCount);

        return mapper.Map<PaginatedList<DealResponse>>(deals);
    }

    public async Task<DealResponse> GetByIdAsync(int id)
    {
        var deal = await dealRepository.GetByIdAsync(id);

        if (deal == null)
        {
            throw new InvalidOperationException("Deal not found.");
        }

        return mapper.Map<DealResponse>(deal);
    }
}
