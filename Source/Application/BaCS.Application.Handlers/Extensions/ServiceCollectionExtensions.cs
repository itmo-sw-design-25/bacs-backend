namespace BaCS.Application.Handlers.Extensions;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationHandlers(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationHandlersAssemblyMarker>()
        );

        return services;
    }
}
