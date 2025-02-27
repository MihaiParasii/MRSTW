using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using MRSTW.DataAccessLayer.Data.Configurations;

namespace MRSTW.DataAccessLayer.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; init; }
    public DbSet<Subcategory> Subcategories { get; init; }
    public DbSet<Deal> Deals { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SubcategoryConfiguration());
        modelBuilder.ApplyConfiguration(new DealConfiguration());
    }
}
