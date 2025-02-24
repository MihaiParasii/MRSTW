using AutoMapper;
using Domain.Models;
using MRSTW.BusinessLogicLayer.Contracts.Deal;

namespace MRSTW.BusinessLogicLayer.Mappings;

public class DealMappingProfile : Profile
{
    public DealMappingProfile()
    {
        CreateMap<Deal, DealResponse>();
        CreateMap<CreateDealRequest, Deal>();
        CreateMap<UpdateDealRequest, Deal>();
    }
}
