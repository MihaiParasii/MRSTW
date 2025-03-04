using Microsoft.EntityFrameworkCore;
using MRSTW.BusinessLogicLayer;
using MRSTW.DataAccessLayer;
using MRSTW.DataAccessLayer.Data;
using MRSTW.DataAccessLayer.Services;

namespace MRSTW.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
        // builder.Configuration.GetConnectionString("DefaultConnection");


        // builder.Services.AddDbContext<AppDbContext>(options =>
        // options.UseNpgsql(connectionString));
        
        // string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"); //??
        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("MRSTW.DataAccessLayer"))
                .LogTo(Console.WriteLine, LogLevel.Information);
        });


        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApiServices();
        builder.Services.AddApplicationServices();


        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();


        // if (app.Environment.IsDevelopment())
        // {
        //     using IServiceScope serviceScope = app.Services.CreateScope();
        //
        //     var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        //     context.Database.Migrate();
        // }

        // await using var serviceScope = app.Services.CreateAsyncScope();

        // await serviceScope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
        // await DatabaseManagementService.MigrationInitializationAsync(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        await app.RunAsync();
    }
}
