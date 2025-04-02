namespace BaCS.Application.Handlers;

using Microsoft.Extensions.DependencyInjection;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApplicationHandlers(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationHandlersAssemblyMarker>()
        );

        return services;
    }
}
