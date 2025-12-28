namespace BaCS.Presentation.BusinessProcess.Extensions;

using Application.Abstractions.Persistence;
using Application.Integrations.Email;
using Elsa.EntityFrameworkCore;
using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Infrastructure.Workflows;
using Microsoft.EntityFrameworkCore;
using Options;
using Persistence.PostgreSQL;
using Persistence.PostgreSQL.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddElsaWorkflowsBusinessProcess(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        var elsaPostgresOptions = configuration.GetSection(nameof(ElsaPostgresOptions)).Get<ElsaPostgresOptions>();

        services
            .AddApplicationServices(configuration, environment)
            .AddElsa(elsa => elsa
                .AddApplicationWorkflows()
                .UseIdentity(identity =>
                    {
                        identity.TokenOptions = options =>
                            options.SigningKey = configuration.GetValue<string>("TokenOptions:SigningKey");

                        identity.UseAdminUserProvider();
                    }
                )
                .UseDefaultAuthentication()
                .UseJavaScript()
                .UseLiquid()
                .UseCSharp()
                .UseHttp(http => http.ConfigureHttpOptions = options => configuration.GetSection("Http").Bind(options))
                .UseWorkflowsApi()
                .UseScheduling()
                .UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(ef =>
                        ef.UsePostgreSql(
                            elsaPostgresOptions.ConnectionString,
                            new ElsaDbContextOptions { SchemaName = elsaPostgresOptions.Schema }
                        )
                    )
                )
                .UseWorkflowManagement(management => management.UseEntityFrameworkCore(ef =>
                        ef.UsePostgreSql(
                            elsaPostgresOptions.ConnectionString,
                            new ElsaDbContextOptions { SchemaName = elsaPostgresOptions.Schema }
                        )
                    )
                )
            );

        return services;
    }

    private static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        var postgresOptions = configuration.GetSection(nameof(PostgresOptions)).Get<PostgresOptions>();

        services.AddEmailIntegration(configuration, environment);
        services.AddDbContext<IBaCSDbContext, BaCSDbContext>(options =>
            {
                options
                    .UseNpgsql(
                        postgresOptions!.ConnectionString,
                        sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", postgresOptions.Schema)
                    )
                    .UseSnakeCaseNamingConvention()
                    .EnableSensitiveDataLogging();

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        );

        return services;
    }
}
