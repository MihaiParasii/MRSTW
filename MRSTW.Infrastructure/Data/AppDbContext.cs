using Microsoft.EntityFrameworkCore;
using MRSTW.Domain.Models;
using MRSTW.Infrastructure.Data.Configurations;

namespace MRSTW.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Subcategory> Subcategories => Set<Subcategory>();
    public DbSet<Deal> Deals => Set<Deal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SubcategoryConfiguration());
        modelBuilder.ApplyConfiguration(new DealConfiguration());
    }
}
