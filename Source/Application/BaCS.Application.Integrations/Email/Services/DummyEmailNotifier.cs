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

    public Task SendReservationPendingApprovalToAdmins(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        IEnumerable<User> admins,
        CancellationToken cancellationToken
    ) => Task.CompletedTask;

    public Task SendReservationApproved(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    ) => Task.CompletedTask;

    public Task SendReservationRejected(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        string reason,
        CancellationToken cancellationToken
    ) => Task.CompletedTask;
}
