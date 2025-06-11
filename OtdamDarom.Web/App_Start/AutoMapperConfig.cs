using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OtdamDarom.Domain.Models;
using OtdamDarom.Web.Requests;

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
            });
        }
    }
}