using Domain.Models.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRSTW.DataAccessLayer.Data.Configurations;

public class DealConfiguration : IEntityTypeConfiguration<DealModel>
{
    public void Configure(EntityTypeBuilder<DealModel> builder)
    {
        throw new NotImplementedException();
    }
}
