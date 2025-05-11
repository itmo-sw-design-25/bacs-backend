namespace BaCS.Presentation.MAUI;

using Microsoft.Maui.Platform;
using Services;
using ViewModels;
using Views;
using Application = Microsoft.Maui.Controls.Application;

public partial class App : Application
{
    private readonly RootVm rootVm;
    public App(RootVm rootVm)
    {
        this.rootVm = rootVm;
        InitializeComponent();
    }

    protected override async void OnStart()
    {
        base.OnStart();
        await InitAsync();
    }

    private async Task InitAsync()
    {
        var authService = rootVm.authentificationService;

        bool isAuthenticated = await authService.UpdateTokensAsync();

        if (isAuthenticated)
        {
            MainPage = new AppShell(); // Основной интерфейс (Shell или NavigationPage)
        }
        else
        {
            MainPage = new NavigationPage(new LoginPage()); // Страница входа
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage());

#if ANDROID
        Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Window.SetNavigationBarColor(Colors.Transparent.ToPlatform());
#endif

    }
}
