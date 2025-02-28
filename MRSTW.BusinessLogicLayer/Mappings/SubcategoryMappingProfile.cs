using AutoMapper;
using Domain.Models.Main;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;

namespace MRSTW.BusinessLogicLayer.Mappings;

public class SubcategoryMappingProfile : Profile
{
    public SubcategoryMappingProfile()
    {
        CreateMap<SubcategoryModel, SubcategoryResponse>();
        CreateMap<CreateSubcategoryRequest, SubcategoryModel>();
        CreateMap<UpdateSubcategoryRequest, SubcategoryModel>();
    }
}
