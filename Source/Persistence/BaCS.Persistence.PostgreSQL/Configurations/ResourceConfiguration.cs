namespace BaCS.Persistence.PostgreSQL.Configurations;

using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder
            .HasOne(x => x.Location)
            .WithMany(x => x.Resources)
            .HasForeignKey(x => x.LocationId);
    }
}
