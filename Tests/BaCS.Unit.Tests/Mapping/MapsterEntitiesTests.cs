namespace BaCS.Unit.Tests.Mapping;

using Application.Contracts.Dto;
using Application.Mapping.Configs;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Core.Entities;
using FluentAssertions;
using Mapster;
using MapsterMapper;

public class MapsterEntitiesTests
{
    private readonly IMapper _mapper;
    private readonly TypeAdapterConfig _config;

    public MapsterEntitiesTests()
    {
        _config = new TypeAdapterConfig();
        _config.Apply(new MapsterEntitiesConfig());
        _mapper = new Mapper(_config);
    }

    [Fact]
    public void ShouldHaveValidConfiguration() => _config.Compile(failFast: true);

    [Theory]
    [AutoData]
    public void ShouldMapFromUserToUserDto_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var user = fixture.Create<User>();

        // Act
        var userDto = _mapper.Map<UserDto>(user);

        // Assert
        user.Should().BeEquivalentTo(userDto);
    }

    [Theory]
    [AutoData]
    public void ShouldMapFromResourceToResourceDto_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var resource = fixture.Build<Resource>().Without(x => x.Location).Create();

        // Act
        var resourceDto = _mapper.Map<ResourceDto>(resource);

        // Assert
        resource.Should().BeEquivalentTo(resourceDto);
    }

    [Theory]
    [AutoData]
    public void ShouldMapFromLocationToLocationDto_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var location = fixture
            .Build<Location>()
            .Without(x => x.Resources)
            .Without(x => x.Admins)
            .Without(x => x.CalendarSettings)
            .Create();

        // Act
        var locationDto = _mapper.Map<LocationDto>(location);

        // Assert
        location.Should().BeEquivalentTo(locationDto);
    }

    [Theory]
    [AutoData]
    public void ShouldMapFromCalendarSettingsToCalendarSettingsDto_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var calendarSettings = fixture
            .Build<CalendarSettings>()
            .Without(x => x.Location)
            .Create();

        // Act
        var calendarSettingsDto = _mapper.Map<CalendarSettingsDto>(calendarSettings);

        // Assert
        calendarSettings.Should().BeEquivalentTo(calendarSettingsDto);
    }

    [Theory]
    [AutoData]
    public void ShouldMapFromReservationToReservationDto_Successful([Frozen] IFixture fixture)
    {
        // Arrange
        var reservation = fixture
            .Build<Reservation>()
            .Without(x => x.Location)
            .Without(x => x.Resource)
            .Without(x => x.User)
            .Create();

        // Act
        var reservationDto = _mapper.Map<ReservationDto>(reservation);

        // Assert
        reservation.Should().BeEquivalentTo(reservationDto);
    }
}
