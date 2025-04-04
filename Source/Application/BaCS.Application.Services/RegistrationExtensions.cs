namespace BaCS.Application.Services;

using Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Services;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<IReservationCalendarValidator, ReservationCalendarValidator>();

        return services;
    }
}
