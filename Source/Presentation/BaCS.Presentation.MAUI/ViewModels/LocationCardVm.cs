namespace BaCS.Presentation.MAUI.ViewModels;

using System.Collections.ObjectModel;
using BaCS.Presentation.MAUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Services;

public class LocationCardVm : ObservableObject
{
    private readonly LocationDto location;

    //TODO Переписать на ResourceVm
    public LocationCardVm(LocationDto location)
    {
        this.location = location;
        Resources = new ObservableCollection<ResourceVm>();
    }

    public string Name { get => location.Name; }

    public string Address { get => location.Address; }

    public string Description { get => location.Description; }

    public string ImageUrl { get => location.ImageUrl; }

    public CalendarSettingsDto CalendarSettings { get => location.CalendarSettings; }
    public ObservableCollection<ResourceVm> Resources { get; }

}
