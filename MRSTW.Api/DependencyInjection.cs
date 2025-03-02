using FluentValidation;
using Microsoft.OpenApi.Models;
using MRSTW.Api.Validators;
using MRSTW.BusinessLogicLayer.Mappings;
using MRSTW.BusinessLogicLayer.Validators;

namespace MRSTW.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        
        services.AddAutoMapper(typeof(CategoryMappingProfile), typeof(SubcategoryMappingProfile),
            typeof(DealMappingProfile));
        
        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "MRSTW API", Version = "v1" }); });
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        
        services.AddValidatorsFromAssemblyContaining<CreateSubcategoryRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateDealRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateSubcategoryRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateCategoryRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateDealRequestValidator>();



        return services;
    }
}
