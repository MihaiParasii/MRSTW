using AutoMapper;
using Domain.Models.Main;
using MRSTW.BusinessLogicLayer.Contracts.Category;

namespace MRSTW.BusinessLogicLayer.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CategoryModel, CategoryResponse>()
            .ForMember(x => x.SubcategoryIds, opt => opt.MapFrom(y => y.Subcategories.Select(x => x.Id)));
        CreateMap<CreateCategoryRequest, CategoryModel>();
        CreateMap<UpdateCategoryRequest, CategoryModel>();
    }
}
