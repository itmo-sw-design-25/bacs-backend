namespace BaCS.Persistence.PostgreSQL.Extensions;

using Application.Abstractions.Persistence;
using Interceptors;
using Microsoft.AspNetCore.Builder;
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
        services.AddDbContext<IBaCSDbContext, BaCSDbContext>(
            (sp, builder) => action.Invoke(
                builder
                    .AddInterceptors(sp.GetRequiredService<AuditingSaveChangesInterceptor>())
            )
        );

        return services;
    }

    public static IServiceCollection AddNpgsqlDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var postgresOptions = configuration.GetSection(nameof(PostgresOptions)).Get<PostgresOptions>();

        services.AddScoped<AuditingSaveChangesInterceptor>();
        services.AddDbContext(
            options =>
            {
                options
                    .UseNpgsql(
                        postgresOptions!.ConnectionString,
                        sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", postgresOptions.Schema)
                    )
                    .UseSnakeCaseNamingConvention()
                    .EnableSensitiveDataLogging();
            }
        );

        services.AddHealthChecks().AddNpgSql(postgresOptions!.ConnectionString, name: "PostgreSQL");

        return services;
    }

    public static async Task MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<BaCSDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
