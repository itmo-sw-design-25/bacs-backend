using Microsoft.Extensions.Logging;

namespace BaCS.Presentation.MAUI;

using Services;
using ViewModels;
using ViewModels.States;
using Views;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
                    fonts.AddFont("Nunito-SemiBold.ttf", "NunitoSemiBold");
                    fonts.AddFont("Nunito-ExtraBold.ttf", "NunitoExtraBold");
                    fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");
                    fonts.AddFont("Nunito-Medium.ttf", "NunitoMedium");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                }
            );

        builder.Services.AddSingleton<UserProfileState>();
        builder.Services.AddSingleton<IAuthentificationService, AuthentificationService>();
        builder.Services.AddSingleton<Client>();
        builder.Services.AddSingleton<SearchResourcesState>();
        builder.Services.AddTransient<SearchResultVm>();
        builder.Services.AddTransient<SearchFilterVm>();
        builder.Services.AddTransient<SearchVm>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LoginPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
