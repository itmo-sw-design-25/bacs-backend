using BaCS.Infrastructure.Observability.HealthChecks;
using BaCS.Infrastructure.Observability.Logging;
using BaCS.Infrastructure.Observability.OpenTelemetry;
using BaCS.Persistence.Minio.Extensions;
using BaCS.Persistence.PostgreSQL.Extensions;
using BaCS.Presentation.API.Extensions;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Host.UseSerilogLogging();

builder
    .Services
    .AddNpgsqlDbContext(configuration)
    .AddMinioStorage(configuration)
    .AddControllers();

builder.Services.AddOpenApi(configuration);

// Observability
builder
    .Services
    .AddHealthChecks(configuration)
    .AddOpenTelemetry(configuration);

var app = builder.Build();

app.UseOpenApi(configuration);
app.UseHttpMetrics();
app.UseSerilogRequestLogging();

app.UseAuthorization();
app.UseRouting();
app.MapControllers();
app.MapMetrics();
app.UsePathBase("/api");

await app.MigrateDatabase();
await app.RunAsync();
await Log.CloseAndFlushAsync();
