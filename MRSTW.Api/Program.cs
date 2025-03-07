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

        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApiServices();
        builder.Services.AddApplicationServices();

        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            await using var serviceScope = app.Services.CreateAsyncScope();
        
            await using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();
        }

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
