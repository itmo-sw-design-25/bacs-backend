namespace BaCS.Application.Integrations.Email.Services;

using Abstractions.Integrations;
using Domain.Core.Entities;

public class DummyEmailNotifier : IEmailNotifier
{
    public Task SendReservationCreated(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    ) => Task.CompletedTask;

    public Task SendReservationUpdated(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    ) => Task.CompletedTask;

    public Task SendReservationCancelled(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    ) => Task.CompletedTask;
}
