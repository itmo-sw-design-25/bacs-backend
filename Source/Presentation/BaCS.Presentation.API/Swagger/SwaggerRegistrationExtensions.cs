namespace BaCS.Presentation.API.Swagger;

using Filters;
using Keycloak.AuthServices.Authentication;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerRegistrationExtensions
{
    public static SwaggerGenOptions ConfigureSwaggerOptions(
        this SwaggerGenOptions options,
        IConfiguration configuration
    )
    {
        options.DescribeAllParametersInCamelCase();
        options.SchemaFilter<ProblemDetailsSchemaFilter>();
        options.OperationFilter<ProblemDetailsOperationFilter>();

        var securityScheme = GetSecurityScheme(configuration);

        options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, [] } });

        return options;
    }

    private static OpenApiSecurityScheme GetSecurityScheme(IConfiguration configuration)
    {
        var keycloakOptions = configuration
            .GetSection("Keycloak")
            .Get<KeycloakAuthenticationOptions>()!;

        var keycloakSecurityScheme = new OpenApiSecurityScheme
        {
            Description = "Аутентификация через Keycloak",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.OAuth2,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Id = "Keycloak",
                Type = ReferenceType.SecurityScheme
            },
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(
                        keycloakOptions.KeycloakUrlRealm + "protocol/openid-connect/auth"
                    ),
                    TokenUrl = new Uri(keycloakOptions.KeycloakTokenEndpoint),
                    Scopes = new Dictionary<string, string>
                    {
                        {
                            keycloakOptions.Resource,
                            "Обязательный скоуп, необходимый для авторизации на BaCS API"
                        }
                    },
                    Extensions = new Dictionary<string, IOpenApiExtension>
                    {
                        ["x-usePkce"] = new OpenApiString("SHA-256")
                    }
                }
            }
        };

        return keycloakSecurityScheme;
    }
}
