namespace BaCS.Application.Mapping.Configs;

using Domain.Core.Entities;
using Handlers.Locations.Commands;
using Handlers.Reservations.Commands;
using Handlers.Resources.Commands;
using Mapster;

public class MapsterCommandsConfig
{
    public MapsterCommandsConfig()
    {
        TypeAdapterConfig<CreateLocationCommand.Command, Location>
            .NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.CalendarSettings, src => src.CalendarSettings);

        TypeAdapterConfig<CreateReservationCommand.Command, Reservation>
            .NewConfig()
            .Map(dest => dest.LocationId, src => src.LocationId)
            .Map(dest => dest.ResourceId, src => src.ResourceId)
            .Map(dest => dest.From, src => src.From)
            .Map(dest => dest.To, src => src.To);

        TypeAdapterConfig<CreateResourceCommand.Command, Resource>
            .NewConfig()
            .Map(dest => dest.LocationId, src => src.LocationId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Equipment, src => src.Equipment);
    }
}
