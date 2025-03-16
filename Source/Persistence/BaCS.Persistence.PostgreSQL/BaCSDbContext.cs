namespace BaCS.Persistence.PostgreSQL;

using Microsoft.EntityFrameworkCore;
using Models;

public class BaCSDbContext : DbContext, IBaCSDbContext
{
    public BaCSDbContext() { }

    public BaCSDbContext(DbContextOptions<BaCSDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; init; }
}
