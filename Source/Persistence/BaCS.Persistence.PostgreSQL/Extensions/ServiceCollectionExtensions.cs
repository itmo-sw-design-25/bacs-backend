namespace BaCS.Persistence.PostgreSQL.Extensions;

using Microsoft.AspNetCore.Builder;
using PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> action
    )
    {
        services.AddDbContext<IBaCSDbContext, BaCSDbContext>((_, builder) => action.Invoke(builder));

        return services;
    }

    public static IServiceCollection AddNpgsqlDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext(
            options =>
            {
                var postgresOptions = configuration.GetSection(nameof(PostgresOptions)).Get<PostgresOptions>();

                options
                    .UseNpgsql(
                        postgresOptions!.ConnectionString,
                        sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", postgresOptions.Schema)
                    )
                    .UseSnakeCaseNamingConvention()
                    .EnableSensitiveDataLogging();
            }
        );

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
