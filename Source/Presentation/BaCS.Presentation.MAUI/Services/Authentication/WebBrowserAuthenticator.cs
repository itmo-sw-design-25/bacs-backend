namespace BaCS.Presentation.MAUI.Services;

using Duende.IdentityModel.Client;
using Duende.IdentityModel.OidcClient.Browser;

public class WebBrowserAuthenticator : IBrowser
{
    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        try
        {
            var authResult =
                await WebAuthenticator.Default.AuthenticateAsync(
                    new Uri(options.StartUrl),
                    new Uri(options.EndUrl));

            var authorizeResponse = ToRawIdentityUrl(options.EndUrl, authResult);

            var url = new RequestUrl("bacs://callback").Create(new Parameters(authResult.Properties));

            return new BrowserResult
            {
                Response = url,
                ResultType = BrowserResultType.Success
            };
        }
        catch (TaskCanceledException)
        {
            return new BrowserResult
            {
                ResultType = BrowserResultType.UserCancel
            };
        }
    }

    private string ToRawIdentityUrl(string redirectUrl, WebAuthenticatorResult result)
    {
        IEnumerable<string> parameters = result.Properties.Select(pair => $"{pair.Key}={pair.Value}");
        var queryString = string.Join("&", parameters);

        return $"{redirectUrl}?{queryString}";
    }
}
