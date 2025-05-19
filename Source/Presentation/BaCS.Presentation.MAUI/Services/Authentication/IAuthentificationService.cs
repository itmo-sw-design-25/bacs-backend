namespace BaCS.Presentation.MAUI.Services;

using Duende.IdentityModel.OidcClient;
using Duende.IdentityModel.OidcClient.Results;

public interface IAuthentificationService
{
    public EventHandler<string> OnAccessTokenUpdated { get; set; }

    public EventHandler<string> OnTokenUpdateRequired { get; set; }

    public EventHandler OnReloginRequested { get; set; }

    public EventHandler<UserDto> OnUserAuthenticationSucced { get; set; }

    public string AccessToken { get; }

    public Task<bool> LoginAsync();

    public Task LogoutAsync(string idToken);

    public Task<bool> UpdateTokensAsync();

    public Task<UserInfoResult> GetUserInfoAsync(string accessToken);
}
