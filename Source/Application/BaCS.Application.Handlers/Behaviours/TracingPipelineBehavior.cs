namespace BaCS.Application.Handlers.Behaviours;

using System.Diagnostics;
using MediatR;

public class TracingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var requestType = typeof(TRequest);
        var requestName = requestType.DeclaringType?.Name ?? requestType.FullName!;
        using var activity = Activity.Current?.Source.StartActivity(requestName);

        return await next();
    }
}
