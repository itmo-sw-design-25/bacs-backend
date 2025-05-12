namespace BaCS.Presentation.MAUI.Services;

using System.Security.Claims;
using System.Timers;
using Application.Contracts.Dto;
using Domain.Core.Entities;
using Duende.IdentityModel.OidcClient;
using Duende.IdentityModel.OidcClient.Results;

public class AuthentificationService : IAuthentificationService, IDisposable
{
    private OidcClient oidcClient;
    private string accessToken;
    private Timer accessTokenRenewer;
    public EventHandler OnLoginSuccess { get; } = delegate { };

    public AuthentificationService()
    {
        var options = new OidcClientOptions
        {
            Authority = "https://bacs.space/keycloak/realms/bacs/",
            ClientId = "bacs-api",
            Scope = "openid bacs-api",
            RedirectUri = "bacs://callback",
            Browser = new WebBrowserAuthenticator(),
            DisablePushedAuthorization = true
        };

        oidcClient = new OidcClient(options);

        accessTokenRenewer = new Timer(TimeSpan.FromMinutes(25).TotalMilliseconds);
        accessTokenRenewer.AutoReset = true;

        Subscribe(true);
    }

    public string AccessToken
    {
        get
        {
            return accessToken;
        }
        private set
        {
            if (accessToken == value)
            {
                return;
            }

            accessToken = value;
        }
    }

    public event AccessTokenUpdatedDelegate OnAccessTokenUpdated;

    public event UserAuthenticationSuccedDelegate OnUserAuthenticationSucced;

    public event TokenUpdateRequiredDelegate OnTokenUpdateRequired;

    public event ReloginRequestedDelegate OnReloginRequested;

    public async Task<bool> LoginAsync()
    {
        try
        {
            var loginResult = await oidcClient.LoginAsync();

            if (loginResult.IsError)
            {
                throw new Exception(loginResult.Error, new Exception(loginResult.ErrorDescription));
            }

            AccessToken = loginResult.AccessToken;
            var refreshToken = loginResult.RefreshToken;
            await SecureStorage.SetAsync("access_token", AccessToken);
            await SecureStorage.SetAsync("refresh_token", refreshToken);

            OnUserAuthenticationSucced.Invoke(CreateUserProfileFromClaims(loginResult.User));

            return true;
        }
        catch (Exception e)
        {
            return false;
        }

        UserDto CreateUserProfileFromClaims(ClaimsPrincipal claims)
        {
            var id = Guid.Parse(claims.FindFirst(ClaimTypes.Sid).Value);
            var name = claims.FindFirst(ClaimTypes.Name).Value;
            var email = claims.FindFirst(ClaimTypes.Email).Value;
            var imageUrl = claims.FindFirst("picture").Value;

            return new UserDto(id, name, email, imageUrl, false);
        }
    }

    //TODO Добавить нормальную обработку ошибок
    public async Task<bool> UpdateTokensAsync()
    {
        try
        {
            var refreshToken = await SecureStorage.GetAsync("refresh_token");

            if (string.IsNullOrEmpty(refreshToken))
            {
                await OnReloginRequested.Invoke();

                return false;
            }

            var result = await oidcClient.RefreshTokenAsync(refreshToken);

            if (result.IsError)
            {
                await OnReloginRequested.Invoke();
            }

            await SecureStorage.SetAsync("refresh_token", result.RefreshToken);
            await SecureStorage.SetAsync("access_token", AccessToken);

            await OnAccessTokenUpdated.Invoke(result.AccessToken);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task LogoutAsync(string idToken)
    {
        var logoutRequest = new LogoutRequest
        {
            IdTokenHint = idToken,
        };

        var result = await oidcClient.LogoutAsync(logoutRequest);

        if (result.IsError)
        {
            SecureStorage.Remove("refresh_token");
            SecureStorage.Remove("access_token");

            await OnReloginRequested.Invoke();
        }
    }

    public async Task<UserInfoResult> GetUserInfoAsync(string accessToken)
    {
        return await oidcClient.GetUserInfoAsync(accessToken);
    }

    private void Subscribe(bool s)
    {
        if (s)
        {
            accessTokenRenewer.Elapsed += UpdateTokensAsync;
        }
        else
        {
            accessTokenRenewer.Elapsed -= UpdateTokensAsync;
        }
    }

    private async void UpdateTokensAsync(object? sender, ElapsedEventArgs e) => await UpdateTokensAsync();

    public void Dispose()
    {
        Subscribe(false);
        accessTokenRenewer.Dispose();
    }
}
