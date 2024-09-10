using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks;

/// <summary>
/// A health check that reports the SHA of the current commit.
/// </summary>
public class GitShaHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = new HealthCheckResult(HealthStatus.Healthy, GitVersionInformation.Sha);

        return Task.FromResult(result);
    }
}