using System.Data.Entity;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("OtdamDarom") { }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<SubcategoryModel> Subcategories { get; set; }
        public DbSet<DealModel> Deals { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserSession> Sessions { get; set; }
    }
}