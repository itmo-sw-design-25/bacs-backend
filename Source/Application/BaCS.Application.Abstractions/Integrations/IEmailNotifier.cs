namespace BaCS.Application.Abstractions.Integrations;

using Domain.Core.Entities;

public interface IEmailNotifier
{
    Task SendReservationCreated(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    );

    Task SendReservationUpdated(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    );

    Task SendReservationCancelled(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    );

    Task SendReservationPendingApprovalToAdmins(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        IEnumerable<User> admins,
        CancellationToken cancellationToken
    );

    Task SendReservationApproved(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    );

    Task SendReservationRejected(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        string reason,
        CancellationToken cancellationToken
    );
}
