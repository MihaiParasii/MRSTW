using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MRSTW.DataAccessLayer.Data.Configurations;

namespace MRSTW.DataAccessLayer.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // private readonly IConfiguration _configuration;

    // public AppDbContext(IConfiguration configuration)
    // {
    //     _configuration = configuration;
    // }
    //
    // public AppDbContext()
    // {
    // }

    // public AppDbContext(IConfiguration configuration) : base()
    // {
    //     _configuration = configuration;
    // }

    public DbSet<CategoryModel> Categories { get; init; }
    public DbSet<SubcategoryModel> Subcategories { get; init; }
    public DbSet<DealModel> Deals { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SubcategoryConfiguration());
        modelBuilder.ApplyConfiguration(new DealConfiguration());
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     // string server = _configuration["server"] ?? "localhost";
    //     // string database = _configuration["database"] ?? "MRSTW";
    //     // string port = _configuration["port"] ?? "5432";
    //     // string username = _configuration["username"] ?? "postgres";
    //     // string password = _configuration["password"] ?? "password";
    //
    //     // string connectionString =
    //     // $"User ID={username};Password={password};Host={server};Port={port};Database={database};Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;";
    //
    //     string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
    //                               _configuration.GetConnectionString("DefaultConnection");
    //
    //
    //     optionsBuilder.UseNpgsql(connectionString);
    // }
}
