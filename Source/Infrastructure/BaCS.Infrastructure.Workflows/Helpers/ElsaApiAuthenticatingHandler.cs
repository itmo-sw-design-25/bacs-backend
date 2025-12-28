namespace BaCS.Infrastructure.Workflows.Helpers;

using System.Net.Http.Headers;
using Elsa.Api.Client.Resources.Identity.Contracts;
using Elsa.Api.Client.Resources.Identity.Requests;
using Microsoft.Extensions.Configuration;

public class ElsaApiAuthenticatingHandler(ILoginApi loginApi, IConfiguration configuration) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var username = configuration.GetValue<string>("ElsaApiOptions:Username");
        var password = configuration.GetValue<string>("ElsaApiOptions:Password");

        var token = await loginApi.LoginAsync(new LoginRequest(username, password), cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
