using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MRSTW.BusinessLogicLayer.Services;

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


        return services;
    }
}
