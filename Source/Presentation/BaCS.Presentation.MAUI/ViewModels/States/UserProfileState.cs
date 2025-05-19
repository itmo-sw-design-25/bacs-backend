namespace BaCS.Presentation.MAUI.ViewModels.States;

using Services;

public class UserProfileState
{
    public UserDto User { get; private set; }

    public UserProfileState(IAuthentificationService authentificationService)
    {
        authentificationService.OnUserAuthenticationSucced += ((sender, dto) => SetUser(dto));
    }

    void SetUser(UserDto user)
    {
        User = user;
    }

    void Clear()
    {
        User = null;
    }
}
