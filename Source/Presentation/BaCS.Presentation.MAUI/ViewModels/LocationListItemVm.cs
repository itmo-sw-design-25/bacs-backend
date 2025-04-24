namespace BaCS.Presentation.MAUI.ViewModels;

using Application.Contracts.Dto;

public class LocationListItemVm : NotifyPropertyChanged
{
    private readonly LocationDto locationDto;

    public LocationListItemVm(LocationDto locationDto)
    {
        this.locationDto = locationDto;
    }

    public string Name { get => locationDto.Name; }

    public string Address { get => locationDto.Address; }

    public string Description { get => locationDto.Description; }

    public string ImageUrl { get => locationDto.ImageUrl; }

    public CalendarSettingsDto CalendarSettings { get => locationDto.CalendarSettings; }

}
