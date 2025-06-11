using System.Data.Entity;
using OtdamDarom.Domain.Models; // Asigură-te că namespace-ul pentru modele este corect

namespace OtdamDarom.BusinessLogic.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection") { }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<SubcategoryModel> Subcategories { get; set; }
        public DbSet<DealModel> Deals { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserSession> Sessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<SubcategoryModel>()
                .HasRequired(s => s.Category)
                .WithMany(c => c.Subcategories)   
                .HasForeignKey(s => s.CategoryId)  
                .WillCascadeOnDelete(false);       
            
            modelBuilder.Entity<DealModel>()
                .HasRequired(d => d.Subcategory) 
                .WithMany()
                                                    
                .HasForeignKey(d => d.SubcategoryId) 
                .WillCascadeOnDelete(false);        // Oprește ștergerea în cascadă dacă ștergi o subcategorie.
            
            modelBuilder.Entity<DealModel>()
                .HasRequired(d => d.User)    
                .WithMany()                         
                .HasForeignKey(d => d.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}