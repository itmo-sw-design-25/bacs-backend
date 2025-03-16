using BaCS.Infrastructure.Observability.HealthChecks;
using BaCS.Infrastructure.Observability.Logging;
using BaCS.Infrastructure.Observability.OpenTelemetry;
using BaCS.Persistence.PostgreSQL.Extensions;
using Prometheus;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Host.UseSerilogLogging();

builder
    .Services
    .AddNpgsqlDbContext(configuration)
    .AddControllers();

builder.Services.AddOpenApi();

// Observability
builder
    .Services
    .AddHealthChecks(configuration)
    .AddOpenTelemetry(configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(
    x =>
    {
        x.Servers = new List<ScalarServer> { new("/api", "BaCS ASP.NET Server") };
        x.WithTitle("BaCS.API").WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    }
);
app.UseHttpMetrics();
app.UseSerilogRequestLogging();

app.UseAuthorization();
app.UseRouting();
app.MapControllers();
app.MapMetrics();
app.UsePathBase("/api");

await app.MigrateDatabase();
await app.RunAsync();
