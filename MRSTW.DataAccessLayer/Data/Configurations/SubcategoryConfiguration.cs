using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRSTW.DataAccessLayer.Data.Configurations;

public class SubcategoryConfiguration : IEntityTypeConfiguration<SubcategoryModel>
{
    public void Configure(EntityTypeBuilder<SubcategoryModel> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(s => s.CategoryModel)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(s => s.CategoryId);
        // .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Deals)
            .WithOne(d => d.SubcategoryModel)
            .HasForeignKey(d => d.SubcategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Subcategories");
    }
}
