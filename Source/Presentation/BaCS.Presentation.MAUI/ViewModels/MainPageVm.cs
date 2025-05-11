namespace BaCS.Presentation.MAUI.ViewModels;

using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services;
using Utils;
using AsyncRelayCommand = CommunityToolkit.Mvvm.Input.AsyncRelayCommand;

public class MainPageVm
{
    private readonly AuthentificationService _authentificationService;
    public ICommand LoginCommand { get; }
    public MainPageVm()
    {
        _authentificationService = new AuthentificationService();
        LoginCommand = new AsyncRelayCommand(_authentificationService.LoginAsync);
    }

    public async void OnLoginClicked(object? sender, EventArgs e)
    {
        var currentAccessToken = await _authentificationService.LoginAsync().ConfigureAwait(false);
        Console.WriteLine($"ТОКЕН ТУТ --> {currentAccessToken.AccessToken}");

    }

}
