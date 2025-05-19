namespace BaCS.Presentation.MAUI.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using States;
using Views;

public class SearchFilterVm : ObservableObject
{
    private SearchResourcesState state;

    private LocationDto selectedLocation;
    private ResourceType selectedResourceType;
    private DateOnly selectedDate;

    public SearchFilterVm(SearchResourcesState state, Client client)
    {
        this.state = state;
        SearchCommand = new RelayCommand(async () => await Search(), CanSearch);
        Locations = client.LocationsGET().Collection.ToList();
    }
    public LocationDto SelectedLocation
    {
        get => selectedLocation;
        set
        {
            SetProperty(ref selectedLocation, value);
            Validate();
        }
    }

    public ResourceType SelectedResourceType
    {
        get => selectedResourceType;
        set
        {
            SetProperty(ref selectedResourceType, value);
            Validate();
        }
    }

    public DateOnly Date
    {
        get => selectedDate;
        set
        {
            SetProperty(ref selectedDate, value);
            Validate();
        }
    }

    public IRelayCommand SearchCommand { get; }

    public List<ResourceType> ResourceTypes { get; } = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().Where(rt => rt != ResourceType.Unspecified).ToList();

    public List<LocationDto> Locations { get; set; }

    public string OfficeTitle => Properties.Resources.Office;
    public string DateTitle => Properties.Resources.Date;
    public string ResourceTypeTitle => Properties.Resources.ResourceType;
    public string FindTitle => Properties.Resources.Find;

    private async Task Search()
    {
        var request = new SearchResourcesRequest()
        {
            SelectedLocation = selectedLocation,
            Date = Date,
            SelectedResourceType = selectedResourceType,
        };

        await state.SendRequestAsync(request);

        await Shell.Current.Navigation.PushAsync(new SearchResultList(new SearchResultVm(state)), true);
    }

    private void Validate()
    {
        SearchCommand.NotifyCanExecuteChanged();
    }
    private bool CanSearch()
    {
        if(selectedLocation == null || selectedResourceType == null || selectedDate == null) return false;
        return true;
    }
}
