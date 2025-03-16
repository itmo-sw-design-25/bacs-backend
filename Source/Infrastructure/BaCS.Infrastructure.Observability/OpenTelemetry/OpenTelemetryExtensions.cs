namespace BaCS.Infrastructure.Observability.OpenTelemetry;

using Options;
using global::OpenTelemetry;
using global::OpenTelemetry.Resources;
using global::OpenTelemetry.Trace;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class OpenTelemetryExtensions
{
    public static OpenTelemetryBuilder AddOpenTelemetry(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var tracingOptions = configuration.GetSection(nameof(TracingOptions)).Get<TracingOptions>()!;

        return services
            .AddOpenTelemetry()
            .WithTracing(
                tracing => tracing
                    .WithResource(tracingOptions)
                    .WithInstrumentation(tracingOptions)
                    .WithExporter(tracingOptions)
            );
    }

    private static TracerProviderBuilder WithResource(this TracerProviderBuilder tracing, TracingOptions options) =>
        tracing
            .AddSource(options.ServiceName)
            .ConfigureResource(
                resourceBuilder => resourceBuilder
                    .AddService(options.ServiceName)
                    .AddEnvironmentVariableDetector()
            );

    private static TracerProviderBuilder WithInstrumentation(
        this TracerProviderBuilder tracing,
        TracingOptions options
    ) =>
        tracing
            .AddEntityFrameworkCoreInstrumentation(opt => opt.SetDbStatementForText = true)
            .AddHttpClientInstrumentation(
                opt =>
                {
                    opt.RecordException = true;
                    opt.EnrichWithException = (activity, exception) =>
                        activity.SetTag("stackTrace", exception.StackTrace);
                }
            )
            .AddAspNetCoreInstrumentation(opt => opt.RecordException = true);

    private static TracerProviderBuilder WithExporter(this TracerProviderBuilder tracing, TracingOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.OtlpCollectorUrl)) return tracing;

        return tracing.AddOtlpExporter(
            opt =>
            {
                opt.Protocol = options.OtlpProtocol;
                opt.Endpoint = new Uri(options.OtlpCollectorUrl);
            }
        );
    }
}
