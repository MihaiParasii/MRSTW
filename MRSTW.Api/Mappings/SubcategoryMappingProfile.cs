using AutoMapper;
using MRSTW.Api.Contracts.Subcategory;
using MRSTW.Domain.Models;

namespace MRSTW.Api.Mappings;

public class SubcategoryMappingProfile : Profile
{
    public SubcategoryMappingProfile()
    {
        CreateMap<Subcategory, SubcategoryResponse>();
        CreateMap<CreateSubcategoryRequest, Subcategory>();
        CreateMap<UpdateSubcategoryRequest, Subcategory>();
    }
}
