using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MRSTW.Application.Common.Interfaces;
using MRSTW.Application.Services;
using MRSTW.Domain.Models;

namespace MRSTW.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<DealService, DealService>();
        services.AddScoped<CategoryService, CategoryService>();
        services.AddScoped<SubcategoryService, SubcategoryService>();


        return services;
    }
}
