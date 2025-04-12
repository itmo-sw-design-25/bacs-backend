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
}
