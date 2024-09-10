using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Checks;

public class AssemblyVersionHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var assemblyVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "Unknown";
        var healthCheckResult = new HealthCheckResult(HealthStatus.Healthy, assemblyVersion);
        return Task.FromResult(healthCheckResult);
    }
}

