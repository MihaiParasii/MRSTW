using AutoMapper;
using Domain.Models;
using MRSTW.Api.Contracts.Subcategory;

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
