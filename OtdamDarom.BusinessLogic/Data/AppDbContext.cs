using System.Data.Entity;
using OtdamDarom.Domain.Models; // Asigură-te că namespace-ul pentru modele este corect

namespace OtdamDarom.BusinessLogic.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection") 
        {
            // Aceste linii sunt setările implicite în multe cazuri, dar e bine să știi de ele.
            // Dacă ai avut probleme, ar putea fi o cauză. Lăsăm comentate pentru moment.
            // this.Configuration.LazyLoadingEnabled = true; // Implicit true, necesar pentru proprietăți virtuale
            // this.Configuration.ProxyCreationEnabled = true; // Implicit true, necesar pentru lazy loading
        }

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
            
            // Poți face asta și pentru subcategorie, dacă vrei ca DealModel să navigheze la SubcategoryModel
            // și să ai o colecție de Deals în SubcategoryModel. Dar e OK și cu WithMany() gol.
            modelBuilder.Entity<DealModel>()
                .HasRequired(d => d.Subcategory) 
                .WithMany() // Dacă nu ai o ICollection<DealModel> în SubcategoryModel, lasă-l așa
                .HasForeignKey(d => d.SubcategoryId) 
                .WillCascadeOnDelete(false);        
            
            // <<<<<<<<<<<<<<<<< CORECȚIE ESENȚIALĂ AICI >>>>>>>>>>>>>>>>>>>>>>
            // Acum că UserModel are ICollection<DealModel> Deals, putem specifica relația
            modelBuilder.Entity<DealModel>()
                .HasRequired(d => d.User)    
                .WithMany(u => u.Deals) // <--- ACUM ESTE CORECT! Specificăm proprietatea de navigație inversă
                .HasForeignKey(d => d.UserId)
                .WillCascadeOnDelete(false); // Asigură-te că logica de ștergere în cascadă este cea dorită
            // <<<<<<<<<<<<<<<<< SFÂRȘIT CORECȚIE >>>>>>>>>>>>>>>>>>>>>>
        }
    }
}