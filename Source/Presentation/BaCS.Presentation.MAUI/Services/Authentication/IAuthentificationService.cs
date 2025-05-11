namespace BaCS.Presentation.MAUI.Services;

using Duende.IdentityModel.OidcClient;
using Duende.IdentityModel.OidcClient.Results;

public interface IAuthentificationService
{
    public event AccessTokenUpdatedDelegate OnAccessTokenUpdated;

    public event TokenUpdateRequiredDelegate OnTokenUpdateRequired;

    public event ReloginRequestedDelegate OnReloginRequested;

    public event UserAuthenticationSuccedDelegate OnUserAuthenticationSucced;

    public string AccessToken { get; }

    public Task<LoginResult> LoginAsync();

    public Task LogoutAsync(string idToken);

    public Task<bool> UpdateTokensAsync();

    public Task<UserInfoResult> GetUserInfoAsync(string accessToken);
}

#region Nested

public delegate Task AccessTokenUpdatedDelegate(string newAccessToken);

public delegate Task<string> TokenUpdateRequiredDelegate();

public delegate Task ReloginRequestedDelegate();

public delegate Task UserAuthenticationSuccedDelegate();

#endregion
