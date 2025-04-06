namespace BaCS.Unit.Tests.TestCases.Reservations;

using AutoFixture;
using AutoFixture.Xunit2;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Contracts.Exceptions;
using Application.Handlers.Reservations.Commands;
using Domain.Core.Entities;
using Domain.Core.Enums;
using Fixture.Attributes;
using FluentAssertions;
using MapsterMapper;
using MockQueryable.NSubstitute;
using NSubstitute;

public class UpdateReservationTests
{
    [Theory]
    [AutoNSubstituteData]
    public async Task UpdateReservation_WithExistingConflictReservation_ThrowsReservationConflictException(
        [Frozen] IFixture fixture,
        [Frozen] IMapper mapper,
        [Frozen] IBaCSDbContext dbContext,
        [Frozen] ICurrentUser currentUser,
        [Frozen] IReservationCalendarValidator calendarValidator,
        [Frozen] [Greedy] Location location,
        [Frozen] Guid userId,
        Guid reservationId
    )
    {
        // Arrange
        var from = fixture.Create<DateTime>();
        var to = from.AddHours(fixture.Create<int>() % 5);

        var command = fixture
            .Build<UpdateReservationCommand.Command>()
            .With(x => x.ReservationId, reservationId)
            .With(x => x.From, from)
            .With(x => x.To, to)
            .Create();

        var existingReservation = fixture
            .Build<Reservation>()
            .With(x => x.Id, reservationId)
            .With(x => x.LocationId, location.Id)
            .With(x => x.UserId, userId)
            .With(x => x.Status, ReservationStatus.Created)
            .Create();

        var conflictingReservation = fixture
            .Build<Reservation>()
            .With(x => x.Id, Guid.CreateVersion7())
            .With(x => x.Status, ReservationStatus.Created)
            .With(x => x.ResourceId, existingReservation.ResourceId)
            .With(x => x.LocationId, location.Id)
            .With(x => x.From, from)
            .With(x => x.To, to)
            .Create();

        var dbSetMock = new[] { conflictingReservation, existingReservation }.AsQueryable().BuildMockDbSet();
        dbContext.Reservations.Returns(dbSetMock);
        dbContext
            .Reservations
            .FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .Returns(existingReservation);

        dbContext.Locations.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>()).Returns(location);
        currentUser.UserId.Returns(userId);

        // Act && Assert
        var handler = new UpdateReservationCommand.Handler(dbContext, calendarValidator, currentUser, mapper);

        await handler
            .Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should()
            .ThrowExactlyAsync<ReservationConflictException>();

        dbContext.Reservations.DidNotReceive().Update(Arg.Any<Reservation>());
        await dbContext.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [AutoNSubstituteData]
    public async Task UpdateReservation_WithoutExistingConflictReservation_Success(
        [Frozen] IFixture fixture,
        [Frozen] IMapper mapper,
        [Frozen] IBaCSDbContext dbContext,
        [Frozen] ICurrentUser currentUser,
        [Frozen] IReservationCalendarValidator calendarValidator,
        [Frozen] [Greedy] Location location,
        [Frozen] Guid userId,
        Guid reservationId
    )
    {
        // Arrange
        var from = fixture.Create<DateTime>();
        var to = from.AddHours(fixture.Create<int>() % 5);

        var command = fixture
            .Build<UpdateReservationCommand.Command>()
            .With(x => x.ReservationId, reservationId)
            .With(x => x.From, from)
            .With(x => x.To, to)
            .Create();

        var existingReservation = fixture
            .Build<Reservation>()
            .With(x => x.Id, reservationId)
            .With(x => x.LocationId, location.Id)
            .With(x => x.UserId, userId)
            .With(x => x.Status, ReservationStatus.Created)
            .Create();

        var dbSetMock = new[] { existingReservation }.AsQueryable().BuildMockDbSet();
        dbContext.Reservations.Returns(dbSetMock);
        dbContext
            .Reservations
            .FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .Returns(existingReservation);

        dbContext.Locations.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>()).Returns(location);
        currentUser.UserId.Returns(userId);

        // Act && Assert
        var handler = new UpdateReservationCommand.Handler(dbContext, calendarValidator, currentUser, mapper);

        await handler
            .Invoking(async h => await h.Handle(command, CancellationToken.None))
            .Should()
            .NotThrowAsync();

        dbContext.Reservations.Received().Update(Arg.Any<Reservation>());
        await dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
