using Domain.Models.Main;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Common.Models;
using MRSTW.BusinessLogicLayer.Contracts.Deal;

namespace MRSTW.BusinessLogicLayer.Services;

public class DealService(IBusinessUnitOfWork unitOfWork) : IDealService
{
    public async Task CreateAsync(CreateDealRequest request, List<string> filePaths)
    {
        var deal = unitOfWork.Mapper.Map<DealModel>(request);

        var validationResult = await unitOfWork.DealValidator.ValidateAsync(deal);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors[0].ToString());
        }

        await unitOfWork.DealRepository.AddAsync(deal);
    }

    public async Task UpdateAsync(UpdateDealRequest request, List<string> filePaths)
    {
        var deal = unitOfWork.Mapper.Map<DealModel>(request);

        var validationResult = await unitOfWork.DealValidator.ValidateAsync(deal);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Invalid deal data.");
        }

        await unitOfWork.DealRepository.UpdateAsync(deal);
    }

    public async Task DeleteAsync(int id)
    {
        var deal = await unitOfWork.DealRepository.GetByIdAsync(id);

        if (deal == null)
        {
            throw new ArgumentException("Deal not found.");
        }

        await unitOfWork.DealRepository.DeleteAsync(deal);
    }

    public async Task<PaginatedList<DealResponse>> GetPaginatedListAsync(int pageSize, int pageCount)
    {
        var deals = await unitOfWork.DealRepository.GetPaginatedListAsync(pageSize, pageCount);

        return unitOfWork.Mapper.Map<PaginatedList<DealResponse>>(deals);
    }

    public async Task<DealResponse> GetByIdAsync(int id)
    {
        var deal = await unitOfWork.DealRepository.GetByIdAsync(id);

        if (deal == null)
        {
            throw new ArgumentException("Deal not found.");
        }

        return unitOfWork.Mapper.Map<DealResponse>(deal);
    }
}
