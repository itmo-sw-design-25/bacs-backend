namespace BaCS.Presentation.MAUI.ViewModels.States;

using Services;

public class SearchResourcesRequest
{
    public DateOnly Date { get; set; }

    public LocationDto SelectedLocation { get; set; }

    public ResourceType SelectedResourceType { get; set; }

    public int Offset { get; set; } = 0;

    public int Limit { get; set; } = 10;
}
