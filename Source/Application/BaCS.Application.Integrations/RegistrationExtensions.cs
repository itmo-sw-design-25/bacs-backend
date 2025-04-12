namespace BaCS.Application.Integrations;

using Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApplicationIntegrations(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        services.AddEmailIntegration(configuration, environment);

        return services;
    }
}
