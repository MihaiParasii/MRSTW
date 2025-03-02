using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.DataAccessLayer.Data;
using MRSTW.DataAccessLayer.Data.Repositories;

namespace MRSTW.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // string? connectionString = configuration.GetConnectionString("Database");
        //
        //
        // string server = configuration["server"] ?? "localhost";
        // string database = configuration["database"] ?? "MRSTW";
        // string port = configuration["port"] ?? "5432";
        // string username = configuration["username"] ?? "postgres";
        // string password = configuration["password"] ?? "password";
        //
        // string connectionString =
        //     $"User ID={username};Password={password};Host={server};Port={port};Database={database};Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;";
        //
        //
        // services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(connectionString); });

        // services.AddScoped<AppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IDealRepository, DealRepository>();
        services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();

        return services;
    }
}
