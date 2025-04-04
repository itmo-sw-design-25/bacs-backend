namespace BaCS.Application.Mapping.Configs;

using Contracts.Dto;
using Domain.Core.Entities;
using Mapster;

public class MapsterEntitiesConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<CalendarSettings, CalendarSettingsDto>()
            .Map(dest => dest.AvailableFrom, src => src.AvailableFrom)
            .Map(dest => dest.AvailableTo, src => src.AvailableTo)
            .Map(dest => dest.AvailableDaysOfWeek, src => src.AvailableDaysOfWeek);

        config
            .NewConfig<Location, LocationDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl)
            .Map(dest => dest.CalendarSettings, src => src.CalendarSettings);

        config
            .NewConfig<Reservation, ReservationDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.LocationId, src => src.LocationId)
            .Map(dest => dest.ResourceId, src => src.ResourceId)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.From, src => src.From)
            .Map(dest => dest.To, src => src.To)
            .Map(dest => dest.Status, src => src.Status);

        config
            .NewConfig<Resource, ResourceDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.LocationId, src => src.LocationId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Equipment, src => src.Equipment)
            .Map(dest => dest.Floor, src => src.Floor)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl);

        config
            .NewConfig<User, UserDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl)
            .Map(dest => dest.EnableEmailNotifications, src => src.EnableEmailNotifications);
    }
}
