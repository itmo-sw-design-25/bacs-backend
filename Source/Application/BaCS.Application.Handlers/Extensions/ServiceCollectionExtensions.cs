namespace BaCS.Application.Handlers.Extensions;

using Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationHandlersAssemblyMarker>()
        );

        return services;
    }
}
