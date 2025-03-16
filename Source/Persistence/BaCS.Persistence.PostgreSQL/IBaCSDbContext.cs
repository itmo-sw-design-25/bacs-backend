namespace BaCS.Persistence.PostgreSQL;

using Microsoft.EntityFrameworkCore;
using Models;

public interface IBaCSDbContext
{
    public DbSet<User> Users { get; init; }
}
