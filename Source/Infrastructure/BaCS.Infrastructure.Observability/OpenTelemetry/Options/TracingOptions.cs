namespace BaCS.Infrastructure.Observability.OpenTelemetry.Options;

using global::OpenTelemetry.Exporter;

public class TracingOptions
{
    public string ServiceName { get; init; }
    public string OtlpCollectorUrl { get; init; }
    public OtlpExportProtocol OtlpProtocol { get; init; }
    public string[] EndpointFilter { get; set; } = [];
}
