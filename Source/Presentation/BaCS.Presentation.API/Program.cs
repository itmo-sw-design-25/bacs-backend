using BaCS.Infrastructure.Observability.HealthChecks;
using BaCS.Infrastructure.Observability.Logging;
using BaCS.Infrastructure.Observability.OpenTelemetry;
using BaCS.Persistence.PostgreSQL.Extensions;
using BaCS.Persistence.PostgreSQL.Options;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Host.UseSerilogLogging();

builder.Services.AddDatabaseContext(
    options =>
    {
        var postgresOptions = configuration.GetSection(nameof(PostgresOptions)).Get<PostgresOptions>();

        options
            .UseNpgsql(
                postgresOptions!.ConnectionString,
                sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", postgresOptions.Schema)
            )
            .UseSnakeCaseNamingConvention()
            .EnableSensitiveDataLogging();
    }
);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Observability
builder
    .Services
    .AddHealthChecks(configuration)
    .AddOpenTelemetry(configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "BaCS API"));
app.UseHttpMetrics();
app.UseSerilogRequestLogging();

app.UseAuthorization();
app.UseRouting();
app.MapControllers();
app.MapMetrics();

await app.MigrateDatabase();

app.Run();
