namespace BaCS.Persistence.PostgreSQL;

using Application.Abstractions;
using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

public class BaCSDbContext : DbContext, IBaCSDbContext
{
    public BaCSDbContext() { }

    public BaCSDbContext(DbContextOptions<BaCSDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; init; }
    public DbSet<Resource> Resources { get; init; }
    public DbSet<Location> Locations { get; init; }
    public DbSet<LocationAdmin> LocationAdmins { get; init; }
    public DbSet<Reservation> Reservations { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaCSDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
