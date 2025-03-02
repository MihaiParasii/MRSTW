using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;
using MRSTW.BusinessLogicLayer.Services;
using MRSTW.BusinessLogicLayer.Validators;

namespace MRSTW.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<DealService, DealService>();
        services.AddScoped<CategoryService, CategoryService>();
        services.AddScoped<SubcategoryService, SubcategoryService>();

        services.AddScoped<IRabbitMqService, RabbitMqService>();

        services.AddValidatorsFromAssemblyContaining<DealValidator>();
        services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
        services.AddValidatorsFromAssemblyContaining<SubcategoryValidator>();
        
        return services;
    }
}
