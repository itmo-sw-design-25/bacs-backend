namespace BaCS.Application.Mapping;

using Configs;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApplicationMapping(this IServiceCollection services)
    {
        services
            .AddSingleton<MapsterEntitiesConfig>()
            .AddSingleton<MapsterCommandsConfig>();

        services.AddMapster();

        return services;
    }
}
