namespace BaCS.Infrastructure.Observability.Logging;

using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Utils;
using Serilog;

public static class LoggingExtensions
{
    public static ConfigureHostBuilder UseSerilogLogging(this ConfigureHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog(
            (context, options) =>
            {
                options
                    .ReadFrom
                    .Configuration(context.Configuration)
                    .Enrich
                    .With<ActivityEnricher>();
            }
        );

        return hostBuilder;
    }
}
