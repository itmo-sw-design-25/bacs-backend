namespace BaCS.Presentation.MAUI.Services;

using Duende.IdentityModel.Client;
using Duende.IdentityModel.OidcClient;
using Duende.IdentityModel.OidcClient.Results;

public class AuthService
{
    private OidcClient oidcClient;

    public void Initialize()
    {
        var options = new OidcClientOptions
        {
            Authority = "https://bacs.space/keycloak/realms/bacs/",
            ClientId = "bacs-api",
            Scope = "openid profile email offline_access",
            RedirectUri = "bacs://callback",
            Browser = new WebBrowserAuthenticator()
        };

        oidcClient = new OidcClient(options);
    }

    public async Task<LoginResult> LoginAsync()
    {
        var loginResult = await oidcClient.LoginAsync();

        if (loginResult.IsError)
        {

            throw new Exception(loginResult.Error, new Exception(loginResult.ErrorDescription));
        }
        var accessToken = loginResult.AccessToken;
        var refreshToken = loginResult.RefreshToken;

        await SecureStorage.SetAsync("access_token", accessToken);
        await SecureStorage.SetAsync("refresh_token", refreshToken);

        return loginResult;
    }

    public async Task LogoutAsync(string idToken)
    {
        var logoutRequest = new LogoutRequest
        {
            IdTokenHint = idToken,
        };

        var endSessionUrl = oidcClient.Options.Authority + "/protocol/openid-connect/logout";

        await Browser.Default.OpenAsync(endSessionUrl, BrowserLaunchMode.SystemPreferred);
    }

    public async Task<UserInfoResult> GetUserInfoAsync(string accessToken)
    {
        return await oidcClient.GetUserInfoAsync(accessToken);
    }
}
