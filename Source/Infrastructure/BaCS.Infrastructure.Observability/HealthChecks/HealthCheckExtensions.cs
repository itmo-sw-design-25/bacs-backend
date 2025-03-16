namespace BaCS.Infrastructure.Observability.HealthChecks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.PostgreSQL.Options;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = services.AddHealthChecks();

        var postgresOptions = configuration
            .GetSection(nameof(PostgresOptions))
            .Get<PostgresOptions>();

        builder.AddNpgSql(postgresOptions!.ConnectionString);

        return services;
    }
}
