using AutoMapper;
using Domain.Models;
using MRSTW.Api.Contracts.Deal;

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
