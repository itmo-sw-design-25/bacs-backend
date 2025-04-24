namespace BaCS.Presentation.MAUI.Models;

using Application.Contracts.Dto;
using Domain.Core.Enums;

public class Resource
{
    public Guid Id;

    public Guid LocationId;

    public string Name;

    public string Description;

    public int Floor;

    public string[] Equipment;

    public ResourceType Type;

    public string ImageUrl;

    public TimeOnly StartTime { get; }

    public TimeOnly EndTime { get; }

    public Resource(ResourceDto dto)
    {
        Id = dto.Id;
        LocationId = dto.LocationId;
        Name = dto.Name;
        Description = dto.Description;
        Floor = dto.Floor;
        Equipment = dto.Equipment;
        Type = dto.Type;
        ImageUrl = dto.ImageUrl;
    }
}
