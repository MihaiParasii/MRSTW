using AutoMapper;
using Domain.Models;
using MRSTW.BusinessLogicLayer.Common.Models;
using MRSTW.BusinessLogicLayer.Contracts.Deal;

namespace MRSTW.BusinessLogicLayer.Mappings;

public class PaginatedListWithDealsMappingProfile : Profile
{
    public PaginatedListWithDealsMappingProfile()
    {
        CreateMap<PaginatedList<Deal>, PaginatedList<DealResponse>>();
    }
}
