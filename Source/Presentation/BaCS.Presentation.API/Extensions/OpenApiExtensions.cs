namespace BaCS.Presentation.API.Extensions;

using Keycloak.AuthServices.Authentication;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Scalar.AspNetCore;
using Swagger;
using Swashbuckle.AspNetCore.Swagger;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "BaCS API",
                            Version = "v1",
                            Description = "API сервиса по бронированию рабочих мест в коворкингах BaCS."
                        }
                    );

                    options.ConfigureSwaggerOptions(configuration);
                }
            );

        return services;
    }

    public static WebApplication UseOpenApi(this WebApplication app, IConfiguration configuration)
    {
        var keycloakOptions = configuration
            .GetSection("Keycloak")
            .Get<KeycloakAuthenticationOptions>(x => x.BindNonPublicProperties = true)!;

        app.UseSwagger();
        app.MapScalarApiReference(
            options => options
                .WithTitle("BaCS.API")
                .AddServer("/api")
                .WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json")
                .WithDefaultHttpClient(ScalarTarget.Shell, ScalarClient.Curl)
                .WithOAuth2Authentication(
                    x =>
                    {
                        x.ClientId = keycloakOptions.Resource;
                        x.Scopes = [keycloakOptions.Resource];
                    }
                )
        );

        return app;
    }

    public static WebApplication WithOpenApiDocument(this WebApplication app, string documentName)
    {
        if (app.Environment.IsProduction()) return app;

        var solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent;

        if (solutionDirectory is null) return app;

        var swaggerProvider = app.Services.GetRequiredService<ISwaggerProvider>();
        var swagger = swaggerProvider.GetSwagger("v1");

        using var stringWriter = new StringWriter();
        swagger.SerializeAsV3(new OpenApiYamlWriter(stringWriter));

        var openApiDocumentPath = Path.Combine(
            solutionDirectory.FullName,
            "openapi",
            $"{documentName}.yaml"
        );
        File.WriteAllText(openApiDocumentPath, stringWriter.ToString());

        return app;
    }
}
