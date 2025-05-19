namespace BaCS.Presentation.MAUI.Services;

using System.Security.AccessControl;
using Application.Contracts.Dto;
using Application.Contracts.Requests;
using Application.Contracts.Responses;
using Domain.Core.Enums;
using RestSharp;
using ResourceType = Domain.Core.Enums.ResourceType;

public static class ApiClientExtensions
{
    #region Locations

    //TODO Проверить параметры для резерваций
    public static async Task<ApiResponce<PaginatedResponse<LocationDto>>> GetLocations(
        this ApiClient restClient,
        IEnumerable<Guid>? reservationsIds = null,
        int? offset = null,
        int? limit = null
    )
    {
        var parameters = new List<Parameter>();

        if (reservationsIds != null) parameters.Add(new QueryParameter("ids", string.Join(",", reservationsIds)));
        if (offset != null) parameters.Add(new QueryParameter("offset", offset.Value.ToString()));
        if (limit != null) parameters.Add(new QueryParameter("limit", limit.Value.ToString()));

        var response =
            await restClient.SendRequestWithBodyResponce<PaginatedResponse<LocationDto>>(
                "/locations",
                Method.Get,
                parameters
            );

        return response;
    }

    public static async Task<ApiResponce<LocationDto>> CreateLocation(
        this ApiClient restClient,
        CreateLocationRequest location
    )
        => await restClient.SendRequestWithBodyResponce<LocationDto>("/locations", Method.Post, body: location);

    public static async Task<ApiResponce<LocationDto>> GetLocationById(this ApiClient restClient, Guid locationId)
        => await restClient.SendRequestWithBodyResponce<LocationDto>($"/locations/{locationId}", Method.Get);

    public static async Task<ApiResponce<LocationDto>> UpdateLocation(
        this ApiClient restClient,
        Guid locationId,
        UpdateLocationRequest location
    )
        => await restClient.SendRequestWithBodyResponce<LocationDto>(
            $"/locations/{locationId}",
            Method.Put,
            body: location
        );

    public static async Task<ApiResponce<bool>> DeleteLocation(this ApiClient restClient, Guid locationId)
        => await restClient.SendRequest($"/locations/{locationId}", Method.Delete);

    //TODO Уточнить насчет фотографий
    public static async Task<ApiResponce<bool>> UpdateLocationPhoto(this ApiClient restClient, Guid locationId)
        => await restClient.SendRequest($"/locations/{locationId}", Method.Put);

    public static async Task<ApiResponce<bool>> DeleteLocationPhoto(this ApiClient restClient, Guid locationId)
        => await restClient.SendRequest($"/locations/{locationId}", Method.Delete);

    public static async Task<ApiResponce<bool>> AddAdminToLocation(
        this ApiClient restClient,
        Guid locationId,
        Guid adminId
    )
        => await restClient.SendRequest($"/locations/{locationId}/admins/{adminId}", Method.Put);

    public static async Task<ApiResponce<bool>> RemoveAdminToLocation(
        this ApiClient restClient,
        Guid locationId,
        Guid adminId
    )
        => await restClient.SendRequest($"/locations/{locationId}/admins/{adminId}", Method.Delete);

    #endregion

    #region Reservations

    public static async Task<ApiResponce<PaginatedResponse<LocationDto>>> GetReservations(
        this ApiClient restClient,
        IEnumerable<Guid>? ids = null,
        IEnumerable<Guid>? userIds = null,
        IEnumerable<Guid>? locationIds = null,
        IEnumerable<Guid>? resourceIds = null,
        IEnumerable<ReservationStatus>? statuses = null,
        DateTime? afterDate = null,
        DateTime? beforeDate = null,
        int? offset = null,
        int? limit = null
    )
    {
        var parameters = new List<Parameter>();

        if (ids != null) parameters.Add(new QueryParameter("ids", string.Join(",", ids)));
        if (userIds != null) parameters.Add(new QueryParameter("userIds", string.Join(",", userIds)));
        if (locationIds != null) parameters.Add(new QueryParameter("locationIds", string.Join(",", locationIds)));
        if (resourceIds != null) parameters.Add(new QueryParameter("resourceIds", string.Join(",", resourceIds)));
        if (statuses != null) parameters.Add(new QueryParameter("statuses", string.Join(",", statuses)));

        if (afterDate != null) parameters.Add(new QueryParameter("afterDate", afterDate.Value.ToString("O")));
        if (beforeDate != null) parameters.Add(new QueryParameter("beforeDate", beforeDate.Value.ToString("O")));


        if (offset != null) parameters.Add(new QueryParameter("offset", offset.Value.ToString()));
        if (limit != null) parameters.Add(new QueryParameter("limit", limit.Value.ToString()));

        var response =
            await restClient.SendRequestWithBodyResponce<PaginatedResponse<LocationDto>>(
                "/reservations",
                Method.Get,
                parameters
            );

        return response;
    }

    public static async Task<ApiResponce<ReservationDto>> CreateReservation(
        this ApiClient restClient,
        CreateReservationRequest reservation
    )
        => await restClient.SendRequestWithBodyResponce<ReservationDto>(
            "/reservations",
            Method.Post,
            body: reservation
        );

    public static async Task<ApiResponce<ReservationDto>> GetReservation(this ApiClient restClient, Guid reservationId)
        => await restClient.SendRequestWithBodyResponce<ReservationDto>($"/reservations/{reservationId}", Method.Get);

    public static async Task<ApiResponce<ReservationDto>> UpdateReservation(
        this ApiClient restClient,
        Guid reservationId,
        UpdateReservationRequest reservation
    )
        => await restClient.SendRequestWithBodyResponce<ReservationDto>(
            $"/reservations/{reservationId}",
            Method.Put,
            body: reservation
        );

    public static async Task<ApiResponce<ReservationDto>> CancelReservation(
        this ApiClient restClient,
        Guid reservationId
    )
        => await restClient.SendRequestWithBodyResponce<ReservationDto>(
            $"/reservations/{reservationId}/Cancel",
            Method.Put
        );

    #endregion

    #region Resources

    public static async Task<ApiResponce<PaginatedResponse<ResourceDto>>> GetResources(
        this ApiClient restClient,
        IEnumerable<Guid>? locationIds = null,
        IEnumerable<ResourceType>? types = null,
        int? offset = null,
        int? limit = null
    )
    {
        var parameters = new List<Parameter>();

        if (locationIds != null) parameters.Add(new QueryParameter("locationIds", string.Join(",", locationIds)));
        if (types != null) parameters.Add(new QueryParameter("types", string.Join(",", types)));
        if (offset != null) parameters.Add(new QueryParameter("offset", offset.Value.ToString()));
        if (limit != null) parameters.Add(new QueryParameter("limit", limit.Value.ToString()));

        var response =
            await restClient.SendRequestWithBodyResponce<PaginatedResponse<ResourceDto>>(
                "/resources",
                Method.Get,
                parameters
            );

        return response;
    }

    public static async Task<ApiResponce<ResourceDto>> CreateResource(
        this ApiClient restClient,
        CreateResourceRequest resource
    )
        => await restClient.SendRequestWithBodyResponce<ResourceDto>("/resources", Method.Post, body: resource);

    public static async Task<ApiResponce<ResourceDto>> GetResourceById(this ApiClient restClient, Guid resourceId)
        => await restClient.SendRequestWithBodyResponce<ResourceDto>($"/resources/{resourceId}", Method.Get);

    public static async Task<ApiResponce<ResourceDto>> UpdateResource(
        this ApiClient restClient,
        Guid resourceId,
        UpdateResourceRequest resource
    )
        => await restClient.SendRequestWithBodyResponce<ResourceDto>(
            $"/resources/{resourceId}",
            Method.Put,
            body: resource
        );

    public static async Task<ApiResponce<bool>> DeleteResource(this ApiClient restClient, Guid resourceId)
        => await restClient.SendRequest($"/resources/{resourceId}", Method.Delete);

    //TODO Уточнить насчет фотографий
    public static async Task<ApiResponce<bool>> UpdateResourcePhoto(this ApiClient restClient, Guid resourceId)
        => await restClient.SendRequest($"/resources/{resourceId}/image", Method.Put);

    public static async Task<ApiResponce<bool>> DeleteResourcePhoto(this ApiClient restClient, Guid resourceId)
        => await restClient.SendRequest($"/resources/{resourceId}/image", Method.Delete);

    #endregion

    #region Users

    public static async Task<ApiResponce<PaginatedResponse<UserDto>>> GetUsers(
        this ApiClient restClient,
        IEnumerable<Guid>? ids = null,
        int? offset = null,
        int? limit = null
    )
    {
        var parameters = new List<Parameter>();

        if (ids != null) parameters.Add(new QueryParameter("userIds", string.Join(",", ids)));
        if (offset != null) parameters.Add(new QueryParameter("offset", offset.Value.ToString()));
        if (limit != null) parameters.Add(new QueryParameter("limit", limit.Value.ToString()));

        var response =
            await restClient.SendRequestWithBodyResponce<PaginatedResponse<UserDto>>("/users", Method.Get, parameters);

        return response;
    }

    public static async Task<ApiResponce<UserDto>> GetUserById(this ApiClient restClient, Guid userId)
        => await restClient.SendRequestWithBodyResponce<UserDto>($"/users/{userId}", Method.Get);

    public static async Task<ApiResponce<UserDto>> UpdateUser(
        this ApiClient restClient,
        Guid userId,
        UpdateUserRequest user
    )
        => await restClient.SendRequestWithBodyResponce<UserDto>($"/users/{userId}", Method.Put, body: user);

    public static async Task<ApiResponce<bool>> DeleteUser(this ApiClient restClient, Guid userId)
        => await restClient.SendRequest($"/users/{userId}", Method.Delete);

    #endregion
}
