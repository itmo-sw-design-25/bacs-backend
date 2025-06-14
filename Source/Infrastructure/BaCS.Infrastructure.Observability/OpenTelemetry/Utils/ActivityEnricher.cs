namespace BaCS.Infrastructure.Observability.OpenTelemetry.Utils;

using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

public class ActivityEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;

        if (activity is null) return;

        logEvent.AddPropertyIfAbsent(new LogEventProperty("SpanId", new ScalarValue(activity.GetSpanId())));
        logEvent.AddPropertyIfAbsent(new LogEventProperty("TraceId", new ScalarValue(activity.GetTraceId())));
        logEvent.AddPropertyIfAbsent(new LogEventProperty("ParentId", new ScalarValue(activity.GetParentId())));
    }
}

internal static class ActivityExtensions
{
    public static string GetSpanId(this Activity activity) => activity?.IdFormat switch
    {
        ActivityIdFormat.Hierarchical => activity?.Id,
        ActivityIdFormat.W3C => activity?.SpanId.ToHexString(),
        _ => null
    } ?? string.Empty;

    public static string GetTraceId(this Activity activity) => activity?.IdFormat switch
    {
        ActivityIdFormat.Hierarchical => activity?.RootId,
        ActivityIdFormat.W3C => activity?.TraceId.ToHexString(),
        _ => null
    } ?? string.Empty;

    public static string GetParentId(this Activity activity) => activity?.IdFormat switch
    {
        ActivityIdFormat.Hierarchical => activity?.ParentId,
        ActivityIdFormat.W3C => activity?.ParentSpanId.ToHexString(),
        _ => null
    } ?? string.Empty;
}
