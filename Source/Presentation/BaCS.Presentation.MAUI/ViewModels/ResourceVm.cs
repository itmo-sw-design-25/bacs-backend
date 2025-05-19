namespace BaCS.Presentation.MAUI.ViewModels;

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Services;

public class ResourceVm : ObservableObject
{
    private readonly ResourceDto resource;
    private TimeSpan AvailableFrom;
    private TimeSpan AvailableTo;

    public ResourceVm(ResourceDto resource, LocationDto parentLocation)
    {
        this.resource = resource;
        AvailableFrom = parentLocation.CalendarSettings.AvailableFrom;
        AvailableTo = parentLocation.CalendarSettings.AvailableTo;
    }

    public string Name
    {
        get => resource.Name;
        set => SetProperty(resource.Name, value, resource, (res, newValue) => res.Name = newValue);
    }

    public string Description
    {
        get => resource.Description;
        set => SetProperty(resource.Description, value, resource, (res, newValue) => res.Description = newValue);
    }

    public int Floor
    {
        get => resource.Floor;
        set => SetProperty(resource.Floor, value, resource, (res, newValue) => res.Floor = newValue);
    }

    public List<string> Equipment
    {
        get => resource.Equipment.ToList();
        set => SetProperty(resource.Equipment, value, resource, (res, newValue) => res.Equipment = newValue);
    }

    public string FloorStr
    {
        get => String.Format("{0} этаж", Floor);
    }

    public ResourceType Type
    {
        get => resource.Type;
        set
        {
            if(resource.Type == value) return;
            resource.Type = value;
            OnPropertyChanged(TypeStr);
        }
    }

    public string TypeStr
    {
        get => resource.Type.GetDisplayName();
    }

    public string DateStr
    {
        get => $"{AvailableFrom:hh\\:mm} - {AvailableTo:hh\\:mm}";
    }

    public string ImageUrl
    {
        get => resource.ImageUrl;
    }

    //TODO Сделать кнопку для бронирования
    public ICommand BookCommand { get; }
}
