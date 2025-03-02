using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRSTW.DataAccessLayer.Data;

namespace MRSTW.DataAccessLayer.Services;

public static class DatabaseManagementService
{
    // public static async Task MigrationInitialization(IApplicationBuilder app)
    // {
    //     await using var serviceScope = app.ApplicationServices.CreateAsyncScope();
    //
    //     await serviceScope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
    // }
}
