namespace BaCS.Persistence.PostgreSQL.Configurations;

using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder) => builder.HasIndex(x => x.Email).IsUnique();
}
