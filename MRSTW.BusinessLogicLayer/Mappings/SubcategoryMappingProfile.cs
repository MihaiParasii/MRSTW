using AutoMapper;
using Domain.Models;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;

namespace MRSTW.BusinessLogicLayer.Mappings;

public class SubcategoryMappingProfile : Profile
{
    public SubcategoryMappingProfile()
    {
        CreateMap<Subcategory, SubcategoryResponse>();
        CreateMap<CreateSubcategoryRequest, Subcategory>();
        CreateMap<UpdateSubcategoryRequest, Subcategory>();
    }
}
