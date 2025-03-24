namespace BaCS.Presentation.API.Extensions;

using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public static class KeycloakExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakWebApiAuthentication(configuration);

        services
            .AddAuthorization(
                options => options.AddPolicy(
                    "DefaultPolicy",
                    builder =>
                        builder
                            .RequireAuthenticatedUser()
                            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                )
            )
            .AddKeycloakAuthorization(configuration);

        return services;
    }
}
