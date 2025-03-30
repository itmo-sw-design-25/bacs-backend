namespace BaCS.Application.Mapping.Extensions;

using Mapster;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationMapping(this IServiceCollection services)
    {
        services.AddMapster();
        services.AddSingleton<MapsterEntitiesConfig>();

        return services;
    }
}
