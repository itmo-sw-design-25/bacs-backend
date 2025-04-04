namespace BaCS.Application.Handlers;

using System.Reflection;
using Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApplicationHandlers(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );

        services.AddMemoryCache();
        services.AddTransient<IClaimsTransformation, ApplicationClaimRolesHandler>();

        return services;
    }
}
