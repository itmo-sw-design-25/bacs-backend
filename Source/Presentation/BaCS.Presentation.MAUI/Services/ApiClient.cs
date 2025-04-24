namespace BaCS.Presentation.MAUI.Services;

using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Contracts.Dto;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using RestSharp;

public class ApiClient
{
    private readonly RestClient restClient;

    public ApiClient(string baseUrl = "https://bacs.space/api")
    {
        restClient = new RestClient(baseUrl);
        SecureStorage.GetAsync("AuthToken").Wait();
        restClient.AddDefaultHeader("Authorization", $"Bearer {System.Environment.GetEnvironmentVariable("API_KEY")}");
    }

    public async Task<T?> SendRequest<T, T2>(string endpoint, Method method = Method.Get, T2 requestData = default(T2)) where T : class where T2 : class
    {
        endpoint = endpoint[0] == '/' ? endpoint : endpoint.Insert(0, "/");
        var request = new RestRequest(endpoint, method);

        if (requestData != null)
        {
            request.AddJsonBody(requestData);
        }

        var responce = await restClient.GetAsync<T>(request);
        return responce;
    }
    public async Task<LocationDto[]> GetLocations()
    {
        var request = new RestRequest("/locations", Method.Get);
        var response = await restClient.GetAsync<LocationDto[]>(request);

        return response;
    }

    public async Task<LocationDto> GetLocation(Guid locationId)
    {
        var request = new RestRequest($"/locations/{locationId}", Method.Get);
        var response = await restClient.GetAsync<LocationDto>(request);
        return response;
    }
}
