namespace BaCS.Presentation.MAUI.ViewModels;

using System.Windows.Input;
using Services;

public class LoginViewModel : NotifyPropertyChanged
{
    private readonly IAuthentificationService authentificationService;


    public LoginViewModel(IAuthentificationService authentificationService)
    {
        this.authentificationService = authentificationService;
        LoginCommand = new Command(async () => await ManualLoginAsync());
        _ = CheckAuthAsync();
    }

    private async Task CheckAuthAsync()
    {
        var result = await authentificationService.UpdateTokensAsync();

        if (result)
        {
            await Shell.Current.GoToAsync("//Home");
        }
    }

    public ICommand LoginCommand { get; }

    private async Task ManualLoginAsync()
    {
        // показываем loader
        var success = await authentificationService.LoginAsync(); // логика авторизации

        if (success)
            await Shell.Current.GoToAsync("//Home");

    }
}
