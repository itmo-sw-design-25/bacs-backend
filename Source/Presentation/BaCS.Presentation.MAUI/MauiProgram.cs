using Microsoft.Extensions.Logging;

namespace BaCS.Presentation.MAUI;

using LocalCache;
using Microsoft.Maui.Handlers;
using Services;

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
                }
            );

        builder.Services.AddSingleton<IAuthentificationService, AuthentificationService>();
        builder.Services.AddSingleton<LocalDatabase>();
        builder.Services.AddSingleton<ApiClient>();

        builder.ConfigureMauiHandlers(
            handlers =>
            {
#if ANDROID
                handlers.AddHandler<Shell, CustomTabBarRenderer>();
#endif
            }
        );
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
