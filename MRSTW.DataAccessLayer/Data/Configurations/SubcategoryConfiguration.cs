using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRSTW.DataAccessLayer.Data.Configurations;

public class SubcategoryConfiguration : IEntityTypeConfiguration<SubcategoryModel>
{
    public void Configure(EntityTypeBuilder<SubcategoryModel> builder)
    {
        throw new NotImplementedException();
    }
}
