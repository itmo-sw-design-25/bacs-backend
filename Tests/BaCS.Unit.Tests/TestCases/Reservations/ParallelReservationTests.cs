namespace BaCS.Unit.Tests.TestCases.Reservations;

using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Abstractions.Workflows;
using Application.Contracts.Exceptions;
using Application.Handlers.Reservations.Commands;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Core.Entities;
using Fixture.Attributes;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.NSubstitute;
using NSubstitute;

public class ParallelReservationTests
{
    [Theory]
    [InlineAutoNSubstituteData(2)]
    [InlineAutoNSubstituteData(5)]
    [InlineAutoNSubstituteData(10)]
    [InlineAutoNSubstituteData(100)]
    public async Task CreateParallelReservationsForSingleResource_ThrowsReservationConflictException(
        int reservationsCount,
        Guid resourceId,
        [Frozen] IFixture fixture,
        [Frozen] IMapper mapper,
        [Frozen] IBaCSDbContext dbContext,
        [Frozen] ICurrentUser currentUser,
        [Frozen] IReservationCalendarValidator calendarValidator,
        [Frozen] IReservationWorkflowService reservationWorkflowService,
        [Frozen] [Greedy] Location location
    )
    {
        // Arrange
        var addedReservations = new List<Reservation>();
        var from = fixture.Create<DateTime>();
        var to = from.AddHours(fixture.Create<int>() % 5);

        var commands = fixture
            .Build<CreateReservationCommand.Command>()
            .With(x => x.ResourceId, resourceId)
            .With(x => x.LocationId, location.Id)
            .With(x => x.From, from)
            .With(x => x.To, to)
            .CreateMany(reservationsCount);

        var dbSetMock = addedReservations.AsQueryable().BuildMockDbSet();
        dbContext.Reservations.Returns(dbSetMock);

        dbContext
            .Reservations
            .AddAsync(Arg.Any<Reservation>(), Arg.Any<CancellationToken>())
            .Returns(
                args =>
                {
                    addedReservations.Add(args.Arg<Reservation>());

                    return ValueTask.FromResult<EntityEntry<Reservation>>(null);
                }
            );
        dbContext.Locations.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>()).Returns(location);

        // Act
        var handler = new CreateReservationCommand.Handler(
            dbContext,
            calendarValidator,
            reservationWorkflowService,
            currentUser,
            mapper
        );
        var tasks = commands.Select(task => handler.Handle(task, CancellationToken.None)).ToArray();

        try { await Task.WhenAll(tasks.Select(task => Task.Run(() => task))); }
        catch (Exception) { }

        // Assert
        var failedTasks = tasks.Where(t => t.IsFaulted).ToArray();
        var successTasks = tasks.Where(t => t.IsCompletedSuccessfully).ToArray();

        successTasks.Should().ContainSingle();
        failedTasks.Should().HaveCount(reservationsCount - 1);
        failedTasks.SelectMany(t => t.Exception!.InnerExceptions).Should().AllBeOfType<ReservationConflictException>();
        addedReservations.Should().ContainSingle();

        await dbContext
            .Reservations
            .Received(1)
            .AddAsync(Arg.Any<Reservation>(), Arg.Any<CancellationToken>());

        await dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineAutoNSubstituteData(2)]
    [InlineAutoNSubstituteData(5)]
    [InlineAutoNSubstituteData(10)]
    [InlineAutoNSubstituteData(100)]
    public async Task CreateParallelReservations_ForDifferentResources_Success(
        int reservationsCount,
        [Frozen] IFixture fixture,
        [Frozen] IMapper mapper,
        [Frozen] IBaCSDbContext dbContext,
        [Frozen] ICurrentUser currentUser,
        [Frozen] IReservationCalendarValidator calendarValidator,
        [Frozen] IReservationWorkflowService reservationWorkflowService,
        [Frozen] [Greedy] Location location
    )
    {
        // Arrange
        var addedReservations = new List<Reservation>();
        var from = fixture.Create<DateTime>();
        var to = from.AddHours(fixture.Create<int>() % 5);

        var commands = fixture
            .Build<CreateReservationCommand.Command>()
            .With(x => x.LocationId, location.Id)
            .With(x => x.From, from)
            .With(x => x.To, to)
            .CreateMany(reservationsCount);

        var dbSetMock = addedReservations.AsQueryable().BuildMockDbSet();
        dbContext.Reservations.Returns(dbSetMock);

        dbContext
            .Reservations
            .AddAsync(Arg.Any<Reservation>(), Arg.Any<CancellationToken>())
            .Returns(
                args =>
                {
                    addedReservations.Add(args.Arg<Reservation>());

                    return ValueTask.FromResult<EntityEntry<Reservation>>(null);
                }
            );
        dbContext.Locations.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>()).Returns(location);

        // Act
        var handler = new CreateReservationCommand.Handler(
            dbContext,
            calendarValidator,
            reservationWorkflowService,
            currentUser,
            mapper
        );
        var tasks = commands.Select(task => handler.Handle(task, CancellationToken.None)).ToArray();

        await Task.WhenAll(tasks.Select(task => Task.Run(() => task)));

        // Assert
        var failedTasks = tasks.Where(t => t.IsFaulted).ToArray();
        var successTasks = tasks.Where(t => t.IsCompletedSuccessfully).ToArray();

        successTasks.Should().HaveCount(reservationsCount);
        failedTasks.Should().HaveCount(0);
        addedReservations.Should().HaveCount(reservationsCount);

        await dbContext
            .Reservations
            .Received(reservationsCount)
            .AddAsync(Arg.Any<Reservation>(), Arg.Any<CancellationToken>());

        await dbContext.Received(reservationsCount).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
