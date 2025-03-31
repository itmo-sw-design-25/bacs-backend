namespace BaCS.Unit.Tests.Mapping;

using Application.Handlers.Locations.Commands;
using Application.Handlers.Reservations.Commands;
using Application.Handlers.Resources.Commands;
using Application.Mapping.Configs;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Core.Entities;
using FluentAssertions;
using Mapster;
using MapsterMapper;

public class MapsterCommandsTests
{
    private readonly IMapper _mapper;
    private readonly TypeAdapterConfig _config;

    public MapsterCommandsTests()
    {
        _config = new TypeAdapterConfig();
        _config.Apply(new MapsterCommandsConfig());
        _mapper = new Mapper(_config);
    }

    [Fact]
    public void ShouldHaveValidConfiguration() => _config.Compile(failFast: true);

    [Theory]
    [AutoData]
    public void ShouldMapFromCreateLocationCommandToLocation_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var createLocationCommand = fixture.Create<CreateLocationCommand.Command>();

        // Act
        var location = _mapper.Map<Location>(createLocationCommand);

        // Assert
        createLocationCommand
            .Should()
            .BeEquivalentTo(location, opt => opt.ExcludingMissingMembers());
    }

    [Theory]
    [AutoData]
    public void ShouldMapFromCreateReservationCommandToReservation_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var createReservationCommand = fixture.Create<CreateReservationCommand.Command>();

        // Act
        var reservation = _mapper.Map<Reservation>(createReservationCommand);

        // Assert
        createReservationCommand
            .Should()
            .BeEquivalentTo(reservation, opt => opt.ExcludingMissingMembers());
    }

    [Theory]
    [AutoData]
    public void ShouldMapFromCreateResourceCommandToResource_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var createResourceCommand = fixture.Create<CreateResourceCommand.Command>();

        // Act
        var resource = _mapper.Map<Resource>(createResourceCommand);

        // Assert
        createResourceCommand
            .Should()
            .BeEquivalentTo(resource, opt => opt.ExcludingMissingMembers());
    }
}
