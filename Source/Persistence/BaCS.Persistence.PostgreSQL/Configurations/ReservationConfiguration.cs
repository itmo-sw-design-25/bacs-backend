namespace BaCS.Persistence.PostgreSQL.Configurations;

using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasIndex(x => x.From);
        builder.HasIndex(x => x.To);
        builder.HasIndex(x => new { x.From, x.To });

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder
            .HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationId);

        builder
            .HasOne(x => x.Resource)
            .WithMany()
            .HasForeignKey(x => x.ResourceId);
    }
}
