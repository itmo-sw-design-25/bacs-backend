namespace BaCS.Application.Mapping;

using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApplicationMapping(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddMapster();

        return services;
    }
}
