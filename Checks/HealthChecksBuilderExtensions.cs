using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Checks;

public static class HealthChecksBuilderExtensions
{
    public static IHealthChecksBuilder AddAssemblyVersion(this IHealthChecksBuilder builder)
    {
        builder.AddCheck<AssemblyVersionHealthCheck>("Assembly Version");
        // return builder.Add(new HealthCheckRegistration("Assembly Version", sp => new AssemblyVersionHealthCheck(), HealthStatus.Healthy, []));

        return builder;
    }
}
