namespace BaCS.Presentation.API.Extensions;

using System.Diagnostics;
using Application.Contracts;
using Application.Handlers.Extensions;
using Application.Mapping.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Middlewares;
using Persistence.Minio.Extensions;
using Persistence.PostgreSQL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddNpgsqlDbContext(configuration)
            .AddMinioStorage(configuration)
            .AddApplicationMapping()
            .AddApplicationHandlers();

        services
            .AddExceptionHandling()
            .AddFluentValidation();

        return services;
    }

    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services
            .AddProblemDetails(
                x =>
                {
                    x.CustomizeProblemDetails = context =>
                    {
                        context.ProblemDetails.Instance =
                            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                        context.ProblemDetails.Extensions.TryAdd("traceId", Activity.Current?.Id);
                    };
                }
            )
            .AddExceptionHandler<ApplicationExceptionHandler>();

        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<IApplicationContractsAssemblyMarker>();
        services.AddFluentValidationRulesToSwagger();

        return services;
    }
}
