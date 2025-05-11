namespace BaCS.Presentation.MAUI.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Core.Enums;

public class SearchFilterVm : ObservableObject
{
    private Location selectedLocation;
    private ResourceType selectedResourceType;
    public Location SelectedLocation
    {
        get => selectedLocation;
        set => SetProperty(ref selectedLocation, value);
    }

    public ResourceType SelectedResourceType
    {
        get => selectedResourceType;
        set => SetProperty(ref selectedResourceType, value);
    }

    public List<ResourceType> ResourceTypes { get; } = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList();
    public ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();
    public DateOnly Date { get; set; }

    public string OfficeTitle => Properties.Resources.Office;
    public string DateTitle => Properties.Resources.Date;
    public string ResourceTypeTitle => Properties.Resources.ResourceType;
}
