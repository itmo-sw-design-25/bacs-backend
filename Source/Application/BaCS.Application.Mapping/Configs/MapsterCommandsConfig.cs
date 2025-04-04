namespace BaCS.Application.Mapping.Configs;

using Domain.Core.Entities;
using Handlers.Locations.Commands;
using Handlers.Reservations.Commands;
using Handlers.Resources.Commands;
using Handlers.Users.Commands;
using Mapster;

public class MapsterCommandsConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<CreateLocationCommand.Command, Location>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.CalendarSettings, src => src.CalendarSettings);

        config
            .NewConfig<CreateReservationCommand.Command, Reservation>()
            .Map(dest => dest.LocationId, src => src.LocationId)
            .Map(dest => dest.ResourceId, src => src.ResourceId)
            .Map(dest => dest.From, src => src.From)
            .Map(dest => dest.To, src => src.To);

        config
            .NewConfig<CreateResourceCommand.Command, Resource>()
            .Map(dest => dest.LocationId, src => src.LocationId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Equipment, src => src.Equipment);

        config
            .NewConfig<CreateUserCommand.Command, User>()
            .Map(dest => dest.Id, src => src.UserId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl)
            .Map(dest => dest.Email, src => src.Email);
    }
}
