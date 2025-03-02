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
        
        string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                                   builder.Configuration.GetConnectionString("DefaultConnection");


        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        
        builder.Services.AddApiServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();


        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();

        // await DatabaseManagementService.MigrationInitialization(app);

        // if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        await app.RunAsync();
    }
}
