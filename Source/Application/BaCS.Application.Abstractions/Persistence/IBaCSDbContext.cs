namespace BaCS.Application.Abstractions.Persistence;

using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

public interface IBaCSDbContext
{
    DbSet<User> Users { get; init; }
    DbSet<Resource> Resources { get; init; }
    DbSet<Location> Locations { get; init; }
    DbSet<LocationAdmin> LocationAdmins { get; init; }
    DbSet<Reservation> Reservations { get; init; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
