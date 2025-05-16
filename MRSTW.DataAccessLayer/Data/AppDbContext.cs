using Domain.Models.Auth;
using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using MRSTW.DataAccessLayer.Data.Configurations;

namespace MRSTW.DataAccessLayer.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CategoryModel> Categories { get; init; }
    public DbSet<SubcategoryModel> Subcategories { get; init; }
    public DbSet<DealModel> Deals { get; init; }
    public DbSet<AppUser> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SubcategoryConfiguration());
        modelBuilder.ApplyConfiguration(new DealConfiguration());
    }
}
