using AutoMapper;
using MRSTW.Api.Contracts.Deal;
using MRSTW.Domain.Models;

namespace MRSTW.Api.Mappings;

public class DealMappingProfile : Profile
{
    public DealMappingProfile()
    {
        CreateMap<Deal, DealResponse>();
        CreateMap<CreateDealRequest, Deal>();
        CreateMap<UpdateDealRequest, Deal>();
    }
}
