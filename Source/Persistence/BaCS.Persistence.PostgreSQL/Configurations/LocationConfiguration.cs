namespace BaCS.Persistence.PostgreSQL.Configurations;

using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder
            .HasMany(x => x.Admins)
            .WithMany()
            .UsingEntity<LocationAdmin>();

        builder
            .OwnsOne(
                x => x.CalendarSettings,
                s =>
                {
                    s
                        .WithOwner(x => x.Location)
                        .HasForeignKey(x => x.LocationId);

                    s.HasKey(x => x.Id);
                    s.ToTable("location_calendar_settings");
                }
            );
    }
}
