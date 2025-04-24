namespace BaCS.Presentation.MAUI.ViewModels;

using System.Collections.ObjectModel;
using Application.Contracts.Dto;
using BaCS.Presentation.MAUI.Models;

public class LocationCardVm : NotifyPropertyChanged
{
    private readonly Location location;

    public LocationCardVm(Location location)
    {
        this.location = location;
        Resources = new ObservableCollection<Resource>(location.Resources);
    }

    public string Name { get => location.Name; }

    public string Address { get => location.Address; }

    public string Description { get => location.Description; }

    public string ImageUrl { get => location.ImageUrl; }

    public CalendarSettingsDto CalendarSettings { get => location.CalendarSettings; }
    public ObservableCollection<Resource> Resources { get; }

}
