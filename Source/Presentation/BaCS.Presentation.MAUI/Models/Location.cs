namespace BaCS.Presentation.MAUI.Models;

using Application.Contracts.Dto;
using Domain.Core.Entities;

public class Location
{
    public Guid Id;

    public string Name;

    public string Address;

    public string Description;

    public string ImageUrl;

    public CalendarSettingsDto CalendarSettings;

    public Guid[] AdminIds;

    public List<Resource> Resources;

    public Location(LocationDto location, IEnumerable<Resource>? resources = null)
    {
        Id = location.Id;
        Name = location.Name;
        Address = location.Address;
        Description = location.Description;
        ImageUrl = location.ImageUrl;
        CalendarSettings = location.CalendarSettings;
        AdminIds = location.AdminIds;
        Resources = resources?.ToList() ?? new List<Resource>();
    }
}
