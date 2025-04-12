namespace BaCS.Application.Integrations.Email;

using Abstractions.Integrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Options;
using Services;

public static class RegistrationExtensions
{
    public static IServiceCollection AddEmailIntegration(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));

        if (environment.IsProduction())
        {
            services.AddScoped<IEmailNotifier, EmailNotifier>();
        }
        else
        {
            services.AddScoped<IEmailNotifier, DummyEmailNotifier>();
        }

        return services;
    }
}
