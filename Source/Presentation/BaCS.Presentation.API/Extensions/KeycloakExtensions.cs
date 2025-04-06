namespace BaCS.Presentation.API.Extensions;

using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public static class KeycloakExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakWebApiAuthentication(
            configuration,
            options => options.MapInboundClaims = false
        );

        services.AddAuthorization(
            options => options.AddPolicy(
                "DefaultPolicy",
                builder => builder
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            )
        );

        return services;
    }
}
