namespace BaCS.Presentation.MAUI.ViewModels.States;

using System;
using System.Threading.Tasks;
using Services;

public class SearchResourcesState
{
    private Client apiClient;

    public SearchResourcesState(Client apiClient)
    {
        this.apiClient = apiClient;
    }

    SearchResourcesRequest? Request { get; set; }

    public SearchResourcesResponce Responce { get; private set; } = new SearchResourcesResponce();

    public bool CanSearchResources { get; private set; } = true;

    public async Task SendRequestAsync(SearchResourcesRequest? request = null)
    {
        try
        {
            if (request != null)
            {
                Request = request;
                Responce.Clear();
            }

            if (Request == null) return;

            var location = await apiClient.LocationsGET2Async(Request.SelectedLocation.Id);

            if (location == null) return;

            var dayOfWeek = (RussianDayOfWeek) (((int) Request.Date.DayOfWeek + 6) % 7);

            if (!location.CalendarSettings.AvailableDaysOfWeek.Contains(dayOfWeek)) return;

            var paginatedResponse = await apiClient.ResourcesGETAsync(
                locationIds: new[] { location.Id },
                offset: Request.Offset,
                limit: Request.Limit
            );

            FillResponce(paginatedResponse, location);
        }
        catch (Exception e) { }
    }

    private  void FillResponce(ResourceDtoPaginatedResponse paginatedResponse, LocationDto location)
    {
        if (Request == null) return;

        var noImageResources = paginatedResponse.Collection.Where(dto => string.IsNullOrWhiteSpace(dto.ImageUrl));
        foreach (var resourceDto in noImageResources)
        {
            resourceDto.ImageUrl = location?.ImageUrl ?? string.Empty;
        }

        Responce.Resources.AddRange(paginatedResponse.Collection.Select(dto => new ResourceVm(dto, location)));
        Request.Offset += Request.Limit;

        if (Request.Offset + Request.Limit >= paginatedResponse.TotalCount)
        {
            Request.Limit = paginatedResponse.TotalCount - Request.Offset;
        }

        CanSearchResources = Responce.TotalCount < paginatedResponse.TotalCount;
    }
}
