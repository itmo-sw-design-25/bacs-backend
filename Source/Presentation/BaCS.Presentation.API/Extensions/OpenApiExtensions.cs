namespace BaCS.Presentation.API.Extensions;

using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

public static class OpenApiExtensions
{
    public static IServiceCollection ApiOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi(
            options =>
            {
                options.AddDocumentTransformer(
                    (document, _, _) =>
                    {
                        document.Info = new OpenApiInfo
                        {
                            Title = "BaCS API",
                            Version = "v1",
                            Description = "API сервиса по бронированию рабочих мест в коворкингах BaCS."
                        };

                        return Task.CompletedTask;
                    }
                );
            }
        );

        return services;
    }

    public static IEndpointRouteBuilder UseOpenApi(this IEndpointRouteBuilder app, IConfiguration configuration)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(
            x =>
            {
                x.Servers = new List<ScalarServer> { new("/api", "BaCS API") };
                x.WithTitle("BaCS.API").WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            }
        );

        return app;
    }
}
