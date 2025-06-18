using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OtdamDarom.Domain.Models;
using OtdamDarom.Web.Requests;
using OtdamDarom.BusinessLogic.Dtos; // <-- Asigură-te că ai acest using pentru DealDto

namespace OtdamDarom.Web
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                // Mapează User
                cfg.CreateMap<CreateUserRequest, UserModel>();
                cfg.CreateMap<UserModel, UserResponse>();
                cfg.CreateMap<UserResponse, UserModel>();
                
                // Mapează Category
                cfg.CreateMap<CreateCategoryRequest, CategoryModel>();
                cfg.CreateMap<UpdateCategoryRequest, CategoryModel>();
                
                // Mapează Subcategory (asigură-te că SubcategoryResponse include CategoryId dacă este necesar pentru mapări)
                cfg.CreateMap<SubcategoryModel, SubcategoryResponse>();
                
                // Mapează CategoryModel la CategoryResponse, incluzând Subcategories
                cfg.CreateMap<CategoryModel, CategoryResponse>()
                    .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(src => src.Subcategories));
                
                // Mapează pentru cererile Create/Update Deal (dacă sunt folosite)
                cfg.CreateMap<CreateDealRequest, DealModel>();
                cfg.CreateMap<UpdateDealRequest, DealModel>();
                
                // Mapează DealModel la DealResponse
                cfg.CreateMap<DealModel, DealResponse>()
                    .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL)); // Poate fi omis dacă numele proprietății este același

                // <<<<<<<<<<<<<<<<< ADAUGĂ ACESTE MAPĂRI PENTRU DealModel și DealDto >>>>>>>>>>>>>>>>>>>>>>
                cfg.CreateMap<DealModel, DealDto>()
                   // Mapează Id
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   // Mapează Name, Description, ImageURL (se pot mapa automat dacă numele corespund)
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                   .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                   .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL))
                   // Mapează SubcategoryId (int?) din DealModel la SelectedSubcategoryId (int) din DealDto
                   // Dacă DealModel.SubcategoryId este null, va fi mapat la 0 în DealDto.SelectedSubcategoryId
                   .ForMember(dest => dest.SelectedSubcategoryId, opt => opt.MapFrom(src => src.SubcategoryId ?? 0))
                   // Mapează CategoryId din Subcategoria DealModel (dacă există) la SelectedCategoryId (int?) din DealDto
                   .ForMember(dest => dest.SelectedCategoryId, opt => opt.MapFrom(src => src.Subcategory != null ? (int?)src.Subcategory.CategoryId : null))
                   // Ignoră proprietățile legate de upload sau ștergere imagine, care nu sunt direct în DealModel
                   .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                   .ForMember(dest => dest.DeleteExistingImage, opt => opt.Ignore());

                cfg.CreateMap<DealDto, DealModel>()
                   // Mapează Id
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   // Mapează Name, Description, ImageURL (se pot mapa automat dacă numele corespund)
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                   .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                   .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL))
                   // Mapează SelectedSubcategoryId (int) din DealDto la SubcategoryId (int?) din DealModel
                   // Conversia de la int la int? este implicită și sigură.
                   .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(src => (int?)src.SelectedSubcategoryId))
                   // Ignoră proprietățile care nu ar trebui să fie mapate direct din DTO la model (ex: CreationDate, UserId)
                   .ForMember(dest => dest.CreationDate, opt => opt.Ignore()) // Setată în controller sau DB
                   .ForMember(dest => dest.UserId, opt => opt.Ignore())       // Setată în controller
                   .ForMember(dest => dest.Subcategory, opt => opt.Ignore())  // Prop. de navigație
                   .ForMember(dest => dest.User, opt => opt.Ignore());        // Prop. de navigație
                // <<<<<<<<<<<<<<<<< SFÂRȘITUL MAPĂRILOR ADĂUGATE >>>>>>>>>>>>>>>>>>>>>>
            });
        }
    }
}