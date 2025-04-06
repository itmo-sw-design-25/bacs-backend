namespace BaCS.Unit.Tests.TestCases.Reservations;

using AutoFixture;
using AutoFixture.Xunit2;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Contracts.Exceptions;
using Application.Handlers.Reservations.Commands;
using Domain.Core.Entities;
using Fixture.Attributes;
using FluentAssertions;
using MapsterMapper;
using MockQueryable.NSubstitute;
using NSubstitute;

public class CreateReservationTests
{
    [Theory]
    [AutoNSubstituteData]
    public async Task CreateReservations_WithConflictingTime_ThrowsReservationConflictException(
        [Frozen] IFixture fixture,
        [Frozen] IMapper mapper,
        [Frozen] IBaCSDbContext dbContext,
        [Frozen] ICurrentUser currentUser,
        [Frozen] IReservationCalendarValidator calendarValidator,
        [Frozen] [Greedy] Location location,
        Guid resourceId
    )
    {
        // Arrange
        var from = fixture.Create<DateTime>();
        var to = from.AddHours(fixture.Create<int>() % 5);

        var command = fixture
            .Build<CreateReservationCommand.Command>()
            .With(x => x.LocationId, location.Id)
            .With(x => x.ResourceId, resourceId)
            .With(x => x.From, from)
            .With(x => x.To, to)
            .Create();

        var existingReservation = mapper.Map<Reservation>(command);

        var reservationsDbSetMock = new[] { existingReservation }.AsQueryable().BuildMockDbSet();
        dbContext.Reservations.Returns(reservationsDbSetMock);
        dbContext.Locations.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>()).Returns(location);

        // Act && Assert
        var handler = new CreateReservationCommand.Handler(dbContext, calendarValidator, currentUser, mapper);

        await handler
            .Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should()
            .ThrowExactlyAsync<ReservationConflictException>();

        await dbContext
            .Reservations
            .DidNotReceive()
            .AddAsync(Arg.Any<Reservation>(), Arg.Any<CancellationToken>());

        await dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [AutoNSubstituteData]
    public async Task CreateReservation_WhenNoOtherReservations_Success(
        [Frozen] IFixture fixture,
        [Frozen] IMapper mapper,
        [Frozen] IBaCSDbContext dbContext,
        [Frozen] ICurrentUser currentUser,
        [Frozen] IReservationCalendarValidator calendarValidator,
        [Frozen] [Greedy] Location location,
        Guid resourceId
    )
    {
        // Arrange
        var from = fixture.Create<DateTime>();
        var to = from.AddHours(fixture.Create<int>() % 5);

        var command = fixture
            .Build<CreateReservationCommand.Command>()
            .With(x => x.ResourceId, resourceId)
            .With(x => x.LocationId, location.Id)
            .With(x => x.From, from)
            .With(x => x.To, to)
            .Create();

        var dbSetMock = Array.Empty<Reservation>().AsQueryable().BuildMockDbSet();
        dbContext.Reservations.Returns(dbSetMock);
        dbContext.Locations.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>()).Returns(location);

        // Act && Assert
        var handler = new CreateReservationCommand.Handler(dbContext, calendarValidator, currentUser, mapper);

        await handler
            .Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should()
            .NotThrowAsync();

        await dbContext
            .Reservations
            .Received(1)
            .AddAsync(
                Arg.Is<Reservation>(x => x.From == from && x.To == to && x.ResourceId == resourceId),
                Arg.Any<CancellationToken>()
            );

        await dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
