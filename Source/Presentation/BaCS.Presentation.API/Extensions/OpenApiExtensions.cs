namespace BaCS.Presentation.API.Extensions;

using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
                {
                    options.DescribeAllParametersInCamelCase();
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "BaCS API",
                        Version = "v1",
                        Description = "API сервиса по бронированию рабочих мест в коворкингах BaCS."
                    });
                }
            );

        return services;
    }

    public static WebApplication UseOpenApi(this WebApplication app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.MapScalarApiReference(
            options =>
            {
                options.OpenApiRoutePattern = "/swagger/{documentName}/swagger.json";
                options.Servers = new List<ScalarServer> { new("/api", "BaCS API") };
                options.WithTitle("BaCS.API").WithDefaultHttpClient(ScalarTarget.Shell, ScalarClient.Curl);
            }
        );

        return app;
    }
}
