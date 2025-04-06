namespace BaCS.Unit.Tests.TestCases.Mapping;

using AutoFixture;
using AutoFixture.Xunit2;
using Application.Handlers.Locations.Commands;
using Application.Handlers.Reservations.Commands;
using Application.Handlers.Resources.Commands;
using Application.Handlers.Users.Commands;
using Application.Mapping.Configs;
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
        var command = fixture.Create<CreateLocationCommand.Command>();

        // Act
        var location = _mapper.Map<Location>(command);

        // Assert
        command
            .Should()
            .BeEquivalentTo(location, opt => opt.ExcludingMissingMembers());
    }

    [Theory]
    [AutoData]
    public void ShouldMapFromCreateReservationCommandToReservation_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var command = fixture.Create<CreateReservationCommand.Command>();

        // Act
        var reservation = _mapper.Map<Reservation>(command);

        // Assert
        command
            .Should()
            .BeEquivalentTo(reservation, opt => opt.ExcludingMissingMembers());
    }

    [Theory]
    [AutoData]
    public void ShouldMapFromCreateResourceCommandToResource_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var command = fixture.Create<CreateResourceCommand.Command>();

        // Act
        var resource = _mapper.Map<Resource>(command);

        // Assert
        command
            .Should()
            .BeEquivalentTo(resource, opt => opt.ExcludingMissingMembers());
    }

    [Theory]
    [AutoData]
    public void ShouldMapFromCreateUserCommandToUser_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var command = fixture.Create<CreateUserCommand.Command>();

        // Act
        var user = _mapper.Map<User>(command);

        // Assert
        command
            .Should()
            .BeEquivalentTo(user, opt => opt.ExcludingMissingMembers());
    }
}
