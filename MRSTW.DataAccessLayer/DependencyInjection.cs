using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.DataAccessLayer.Data;
using MRSTW.DataAccessLayer.Data.Repositories;

namespace MRSTW.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        

        // services.AddScoped<AppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IDealRepository, DealRepository>();
        services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();

        return services;
    }
}
