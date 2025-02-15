using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRSTW.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using MRSTW.Application.Common.Interfaces;
using MRSTW.Infrastructure.Data.Repositories;

namespace MRSTW.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(connectionString); });

        services.AddScoped<AppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IDealRepository, DealRepository>();
        services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();

        return services;
    }
}
