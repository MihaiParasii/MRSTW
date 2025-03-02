using AutoMapper;
using Domain.Models.Main;
using MRSTW.BusinessLogicLayer.Contracts.Category;

namespace MRSTW.BusinessLogicLayer.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CategoryModel, CategoryResponse>();
        CreateMap<CreateCategoryRequest, CategoryModel>();
        CreateMap<UpdateCategoryRequest, CategoryModel>();
    }
}
