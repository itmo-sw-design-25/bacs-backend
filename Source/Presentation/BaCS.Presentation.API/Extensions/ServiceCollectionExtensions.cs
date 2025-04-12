namespace BaCS.Presentation.API.Extensions;

using System.Diagnostics;
using Application.Contracts;
using Application.Handlers;
using Application.Integrations;
using Application.Mapping;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Middlewares;
using Persistence.Minio;
using Persistence.PostgreSQL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        services
            .AddNpgsqlDbContext(configuration)
            .AddMinioStorage(configuration)
            .AddApplicationMapping()
            .AddApplicationServices()
            .AddApplicationIntegrations(configuration, environment)
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
