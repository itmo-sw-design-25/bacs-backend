namespace BaCS.Infrastructure.Observability.HealthChecks;

using global::HealthChecks.UI.Client;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var keycloakOptions = configuration
            .GetSection("Keycloak")
            .Get<KeycloakAuthenticationOptions>(x => x.BindNonPublicProperties = true)!;

        services
            .AddHealthChecks()
            .AddOpenIdConnectServer(oidcSvrUri: new Uri(keycloakOptions.KeycloakUrlRealm), name: "Keycloak");

        services
            .AddHealthChecksUI()
            .AddInMemoryStorage();

        return services;
    }

    public static WebApplication MapHealthChecks(this WebApplication app, IConfiguration configuration)
    {
        app.MapHealthChecks(
            "/healthz",
            new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse }
        );

        app.MapHealthChecksUI(options => options.ResourcesPath = "/healthchecks-resources");

        return app;
    }
}
