namespace BACS.Application.Integrations.DaData;

using BaCS.Application.Abstractions.Integrations;
using BaCS.Application.Integrations.DaData.Services;
using Dadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Options;

public static class RegistrationExtensions
{
    public static IServiceCollection AddDaDataIntegration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var optionsSection = configuration.GetSection(nameof(DaDataOptions));
        var options = optionsSection.Get<DaDataOptions>();

        services.Configure<DaDataOptions>(optionsSection);

        services.AddSingleton(_ => new SuggestClientAsync(options.ApiKey));
        services.AddScoped<IAddressSuggestionsService, AddressSuggestionsService>();

        return services;
    }
}
