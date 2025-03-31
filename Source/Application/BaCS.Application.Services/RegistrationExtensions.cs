namespace BaCS.Application.Services;

using Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Services;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();

        return services;
    }
}
