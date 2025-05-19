namespace BaCS.Presentation.MAUI;

using Microsoft.Maui.Platform;
using Services;
using ViewModels;
using Views;
using Application = Microsoft.Maui.Controls.Application;

public partial class App : Application
{
    private IServiceProvider serviceProvider;
    public App(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;

        InitializeComponent();

        MainPage = new ContentPage
        {
            Content = new ActivityIndicator
            {
                IsRunning = true,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }
        };

    }

    protected override async void OnStart()
    {
        base.OnStart();
        await InitAsync();
    }

    private async Task InitAsync()
    {
        var authService = serviceProvider.GetService<IAuthentificationService>();

        if (authService == null) return;

        bool isAuthenticated = await authService.UpdateTokensAsync();

        MainPage = new AppShell();

        if (isAuthenticated)
        {
           await Shell.Current.GoToAsync($"//Home");
        }
        else
        {
            await Shell.Current.GoToAsync($"//Login");
        }
    }

}
