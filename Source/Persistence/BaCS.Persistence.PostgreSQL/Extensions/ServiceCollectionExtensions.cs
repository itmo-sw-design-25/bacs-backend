namespace BaCS.Persistence.PostgreSQL.Extensions;

using Microsoft.AspNetCore.Builder;
using PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseContext(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> action
    )
    {
        services.AddDbContext<IBaCSDbContext, BaCSDbContext>((_, builder) => action.Invoke(builder));

        return services;
    }

    public static async Task MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<BaCSDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
    }
}
