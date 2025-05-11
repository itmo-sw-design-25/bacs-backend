namespace BaCS.Presentation.MAUI.Services;

using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using RestSharp;

public class ApiClient : IDisposable
{
    private RestClient? restClient { get; set; }
    private IAuthentificationService authentificationService { get; set; }

    public ApiClient(IAuthentificationService service)
    {
        authentificationService = service;
        Subscibe(true);
    }

    public async Task InitializeAsync(string baseUrl = "https://bacs.space/api")
    {
        restClient = new RestClient(baseUrl);
        var accessToken = SecureStorage.GetAsync("access_token").Result;
        //restClient.AddDefaultHeader("Authorization", $"Bearer {accessToken}");
    }

    public async Task<ApiResponce<T>> SendRequestWithBodyResponce<T>(string url, Method method, IEnumerable<Parameter> queryParams = null, object? body = null) where T : class
    {
        var request = new RestRequest(url, method);
        request.AddHeader("Authorization", $"Bearer {authentificationService.AccessToken}");

        if (queryParams != null)
            foreach (var param in queryParams)
                request.AddParameter(param);

        if (body != null)
            request.AddJsonBody(body);

        try
        {
            var response = await restClient.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(response.Content))
                {
                    var result = JsonConvert.DeserializeObject<T>(response.Content);
                    return new ApiResponce<T>(result);
                }

                throw new HttpRequestException("Null response");
            }
            throw new HttpRequestException($"Error: {response.StatusCode} - {response.ErrorMessage}");
        }
        catch (Exception exception)
        {
            return new ApiResponce<T>(exception);
        }
    }

    public async Task<ApiResponce<T>> SendRequestWithBodyRequest<T>(string url, Method method, IEnumerable<Parameter> queryParams = null, object? body = null) where T : class
    {
        var request = new RestRequest(url, method);
        request.AddHeader("Authorization", $"Bearer {authentificationService.AccessToken}");

        if (queryParams != null)
            foreach (var param in queryParams)
                request.AddParameter(param);

        if (body != null)
            request.AddJsonBody(body);

        try
        {
            var response = await restClient.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(response.Content))
                {
                    var result = JsonConvert.DeserializeObject<T>(response.Content);
                    return new ApiResponce<T>(result);
                }

                throw new HttpRequestException("Null response");
            }
            throw new HttpRequestException($"Error: {response.StatusCode} - {response.ErrorMessage}");
        }
        catch (Exception exception)
        {
            return new ApiResponce<T>(exception);
        }
    }

    public async Task<ApiResponce<bool>> SendRequest(string url, Method method, Dictionary<string, string>? queryParams = null, object? body = null)
    {
        var request = new RestRequest(url, method);
        request.AddHeader("Authorization", $"Bearer {authentificationService.AccessToken}");

        if (queryParams != null)
            foreach (var (key, value) in queryParams)
                request.AddQueryParameter(key, value);

        if (body != null)
            request.AddJsonBody(body);
        try
        {
            var response = await restClient.ExecuteAsync(request);

            return new ApiResponce<bool>(response.IsSuccessStatusCode);
        }
        catch (Exception exception)
        {
            return new ApiResponce<bool>(exception);
        }
    }

    public void Dispose()
    {
        Subscibe(false);
        restClient?.Dispose();
    }

    private void Subscibe(bool s)
    {
        if (s)
        {
            authentificationService.OnAccessTokenUpdated += UpdateAccessToken;
        }
        else
        {
            authentificationService.OnAccessTokenUpdated -= UpdateAccessToken;
        }
    }

    private async Task UpdateAccessToken(string token)
    {
        if (restClient != null)
        {
            restClient.AddDefaultHeader("Authorization", $"Bearer {token}");
        }
    }
}
