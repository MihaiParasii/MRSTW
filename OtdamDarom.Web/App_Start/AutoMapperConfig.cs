using AutoMapper;
using OtdamDarom.Domain.Models;
using OtdamDarom.Web.Requests;
using OtdamDarom.BusinessLogic.Dtos;

namespace OtdamDarom.Web
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CreateUserRequest, UserModel>();
                cfg.CreateMap<UserModel, UserResponse>();
                cfg.CreateMap<UserResponse, UserModel>();
                
                cfg.CreateMap<CreateCategoryRequest, CategoryModel>();
                cfg.CreateMap<UpdateCategoryRequest, CategoryModel>();
                
                cfg.CreateMap<SubcategoryModel, SubcategoryResponse>();
                
                cfg.CreateMap<CategoryModel, CategoryResponse>()
                    .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(src => src.Subcategories));
                
                cfg.CreateMap<CreateDealRequest, DealModel>();
                cfg.CreateMap<UpdateDealRequest, DealModel>();
                
                cfg.CreateMap<DealModel, DealResponse>()
                    .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL));
                
                cfg.CreateMap<DealModel, DealDto>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                   .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                   .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL))
                   .ForMember(dest => dest.SelectedSubcategoryId, opt => opt.MapFrom(src => src.SubcategoryId ?? 0))
                   .ForMember(dest => dest.SelectedCategoryId, opt => opt.MapFrom(src => src.Subcategory != null ? (int?)src.Subcategory.CategoryId : null))
                   .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                   .ForMember(dest => dest.DeleteExistingImage, opt => opt.Ignore());

                cfg.CreateMap<DealDto, DealModel>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                   .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                   .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL))
                   .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(src => (int?)src.SelectedSubcategoryId))
                   .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                   .ForMember(dest => dest.UserId, opt => opt.Ignore())
                   .ForMember(dest => dest.Subcategory, opt => opt.Ignore())
                   .ForMember(dest => dest.User, opt => opt.Ignore());
            });
        }
    }
}