namespace BaCS.Application.Abstractions;

using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

public interface IBaCSDbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<Resource> Resources { get; init; }
    public DbSet<Location> Locations { get; init; }
    public DbSet<LocationAdmin> LocationAdmins { get; init; }
    public DbSet<Reservation> Reservations { get; init; }
}
