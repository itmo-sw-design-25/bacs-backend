namespace BaCS.Application.Services.Extensions;

using Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();

        return services;
    }
}
