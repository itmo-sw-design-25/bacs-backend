namespace BaCS.Presentation.MAUI.ViewModels;

using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services;
using Utils;
using AsyncRelayCommand = CommunityToolkit.Mvvm.Input.AsyncRelayCommand;

public class MainPageVm
{
    private readonly AuthService authService;
    public ICommand LoginCommand { get; }
    public MainPageVm()
    {
        authService = new AuthService();
        authService.Initialize();
        LoginCommand = new AsyncRelayCommand(authService.LoginAsync);
    }

    public async void OnLoginClicked(object? sender, EventArgs e)
    {
        var currentAccessToken = await authService.LoginAsync().ConfigureAwait(false);
        Console.WriteLine($"ТОКЕН ТУТ --> {currentAccessToken.AccessToken}");

    }

}
