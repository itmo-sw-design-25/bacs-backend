using BaCS.Infrastructure.Observability.HealthChecks;
using BaCS.Infrastructure.Observability.Logging;
using BaCS.Infrastructure.Observability.OpenTelemetry;
using BaCS.Persistence.PostgreSQL;
using BaCS.Presentation.API.Extensions;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Host.UseSerilogLogging();

builder.Services.AddControllers();
builder.Services.AddApplication(configuration);

builder.Services.AddOpenApi(configuration);

// Observability
builder
    .Services
    .AddHealthChecks(configuration)
    .AddOpenTelemetry(configuration);

builder.Services.AddAuthentication(configuration);

var app = builder.Build();

app
    .UseOpenApi(configuration)
    .UseHttpMetrics()
    .UseSerilogRequestLogging();

app
    .UseExceptionHandler(_ => { })
    .UsePathBase("/api")
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.MapHealthChecks(configuration);
app.MapControllers();
app.MapMetrics();

await app.MigrateDatabase();
await app.RunAsync();
await Log.CloseAndFlushAsync();
