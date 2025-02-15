using AutoMapper;
using MRSTW.Api.Contracts.Category;
using MRSTW.Domain.Models;

namespace MRSTW.Api.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryResponse>();
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<UpdateCategoryRequest, Category>();
    }
}
