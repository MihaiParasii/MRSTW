using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRSTW.DataAccessLayer.Data.Configurations;

public class DealConfiguration : IEntityTypeConfiguration<DealModel>
{
    public void Configure(EntityTypeBuilder<DealModel> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.UserId)
            .IsRequired();

        builder.Property(d => d.PhotoPaths)
            .IsRequired();

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(d => d.CreationDate)
            .IsRequired();

        builder.HasOne(d => d.SubcategoryModel)
            .WithMany(s => s.Deals)
            .HasForeignKey(d => d.SubcategoryId);
        // .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Deals");
    }
}
