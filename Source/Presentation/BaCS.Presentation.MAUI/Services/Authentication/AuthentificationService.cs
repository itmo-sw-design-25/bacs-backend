namespace BaCS.Presentation.MAUI.Services;

using System.Security.Claims;
using System.Timers;

using Duende.IdentityModel.OidcClient;
using Duende.IdentityModel.OidcClient.Results;
using Microsoft.IdentityModel.JsonWebTokens;
using ViewModels.States;

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

    public EventHandler<string> OnAccessTokenUpdated { get; set; } = delegate { };

    public EventHandler<string> OnTokenUpdateRequired { get; set; } = delegate { };

    public EventHandler OnReloginRequested { get; set; } = delegate { };

    public EventHandler<UserDto> OnUserAuthenticationSucced { get; set; } = delegate { };

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

            OnUserAuthenticationSucced.Invoke(this, CreateUserProfileFromClaims(loginResult.User));

            return true;
        }
        catch (Exception e)
        {
            return false;
        }

        UserDto CreateUserProfileFromClaims(ClaimsPrincipal claims)
        {
            var rawId = claims.FindFirst(JwtRegisteredClaimNames.Sid);
            var rawName = claims.FindFirst(JwtRegisteredClaimNames.Name);
            var rawEmail = claims.FindFirst(JwtRegisteredClaimNames.Email);
            var rawImage = claims.FindFirst(JwtRegisteredClaimNames.Picture);

            var id = Guid.Parse(rawId.Value);
            var name = rawName.Value;
            var email = rawEmail.Value;
            var imageUrl = rawImage.Value;

            return new UserDto()
            {
                Id = id,
                Name = name,
                Email = email,
                ImageUrl = imageUrl
            };
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
                OnReloginRequested.Invoke(this, EventArgs.Empty);

                return false;
            }

            var result = await oidcClient.RefreshTokenAsync(refreshToken);

            if (result.IsError)
            {
                OnReloginRequested.Invoke(this, EventArgs.Empty);
            }

            await SecureStorage.SetAsync("refresh_token", result.RefreshToken);
            await SecureStorage.SetAsync("access_token", AccessToken);

            OnAccessTokenUpdated.Invoke(this, result.AccessToken);

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

            OnReloginRequested.Invoke(this, EventArgs.Empty);
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
