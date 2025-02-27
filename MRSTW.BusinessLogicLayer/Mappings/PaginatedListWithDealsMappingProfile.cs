using AutoMapper;
using Domain.Models.Main;
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
