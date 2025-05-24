using System.Collections.Generic;
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
            });
            
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CreateCategoryRequest, CategoryModel>();
                cfg.CreateMap<UpdateCategoryRequest, CategoryModel>();

                cfg.CreateMap<CategoryModel, CategoryResponse>()
                    .ForMember(dest => dest.SubcategoryNames,
                        opt => opt.MapFrom(src => src.Subcategories != null
                            ? src.Subcategories.ConvertAll(s => s.Name)
                            : new List<string>()));
            });
        }
    }
}