namespace BaCS.Application.Abstractions.Workflows;

using Domain.Core.Entities;

/// <summary>
///     Интерфейс для управления бизнес-процессом резервирования.
/// </summary>
public interface IReservationWorkflowService
{
    /// <summary>
    ///     Запустить workflow для новой резервации.
    /// </summary>
    Task StartReservationApprovalWorkflow(Reservation reservation, CancellationToken cancellationToken = default);
}
