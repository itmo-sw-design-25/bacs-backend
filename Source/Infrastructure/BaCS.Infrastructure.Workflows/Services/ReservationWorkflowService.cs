namespace BaCS.Infrastructure.Workflows.Services;

using Application.Abstractions.Workflows;
using Application.Contracts.Exceptions;
using Domain.Core.Entities;
using Elsa.Api.Client.Resources.WorkflowDefinitions.Contracts;
using Elsa.Api.Client.Resources.WorkflowDefinitions.Requests;
using Elsa.Api.Client.Shared.Models;
using Microsoft.Extensions.Logging;
using Workflows;

/// <summary>
/// Сервис для управления бизнес-процессом резервирования.
/// </summary>
public class ReservationWorkflowService(
    IExecuteWorkflowApi workflowClient,
    ILogger<ReservationWorkflowService> logger
) : IReservationWorkflowService
{
    public async Task StartReservationApprovalWorkflow(
        Reservation reservation,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation(
            "Starting reservation approval workflow for reservation {ReservationId}",
            reservation.Id
        );

        var input = new Dictionary<string, object>
        {
            ["ReservationId"] = reservation.Id,
            ["LocationId"] = reservation.LocationId
        };

        var result = await workflowClient.ExecuteAsync(
            ReservationApprovalWorkflow.WorkflowDefinitionId,
            new ExecuteWorkflowDefinitionRequest
            {
                Input = input,
                VersionOptions = VersionOptions.Latest
            },
            cancellationToken
        );

        await EnsureSuccessStatusCode(result, ReservationApprovalWorkflow.WorkflowDefinitionId, reservation);

        logger.LogInformation(
            "Status code {StatusCode} returned for Workflow {WorkflowDefinitionId}.",
            result.StatusCode,
            ReservationApprovalWorkflow.WorkflowDefinitionId
        );
    }

    private async Task EnsureSuccessStatusCode(
        HttpResponseMessage result,
        string definitionId,
        Reservation reservation
    )
    {
        if (result.IsSuccessStatusCode) return;

        var content = await result.Content.ReadAsStringAsync();

        logger.LogError(
            "Failed to execute Workflow {WorkflowDefinitionId} for reservation {ReservationId}. Status code: {StatusCode}. Content: {Content}",
            definitionId,
            reservation.Id,
            result.StatusCode,
            content
        );

        throw new BusinessRulesException("Ошибка выполнения бизнес-процесса");
    }
}
