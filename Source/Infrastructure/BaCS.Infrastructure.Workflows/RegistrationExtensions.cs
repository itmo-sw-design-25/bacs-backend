namespace BaCS.Infrastructure.Workflows;

using Activities;
using Application.Abstractions.Workflows;
using Elsa.Api.Client.Extensions;
using Elsa.Api.Client.Resources.Identity.Contracts;
using Elsa.Api.Client.Resources.WorkflowDefinitions.Contracts;
using Elsa.Extensions;
using Elsa.Features.Services;
using Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Workflows;

public static class RegistrationExtensions
{
    public static IModule AddApplicationWorkflows(this IModule elsa)
    {
        elsa.AddActivitiesFrom<SendPendingApprovalEmailActivity>();
        elsa.AddWorkflowsFrom<ReservationApprovalWorkflow>();

        return elsa;
    }

    public static IServiceCollection AddWorkflowsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var elsaBaseUrl = configuration.GetValue<string>("ElsaApiOptions:Uri");
        var elsaBaseAddress = new Uri(elsaBaseUrl);

        services.AddTransient<ElsaApiAuthenticatingHandler>();

        services
            .AddApiClient<ILoginApi>(opt => opt.BaseAddress = elsaBaseAddress)
            .AddApiClient<IExecuteWorkflowApi>(opt =>
                {
                    opt.BaseAddress = elsaBaseAddress;
                    opt.AuthenticationHandler = typeof(ElsaApiAuthenticatingHandler);
                }
            );

        services.AddScoped<IReservationWorkflowService, ReservationWorkflowService>();

        return services;
    }
}
