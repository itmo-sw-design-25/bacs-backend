namespace BaCS.Persistence.Minio.HealthCheck;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Options;

public class MinioHealthCheck(IOptions<MinioOptions> options) : IHealthCheck
{
    private readonly MinioOptions _options = options.Value;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(_options.Url) };

            var response = await httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, "/minio/health/live"),
                cancellationToken
            );

            response.EnsureSuccessStatusCode();

            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Degraded(e.Message);
        }
    }
}
