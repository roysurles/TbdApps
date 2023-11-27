namespace RecipeApp.CoreApi.Features.HealthChecks;

/// <summary>
/// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0
/// </summary>
public class SampleHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var dict = new Dictionary<string, object>
            {
                { "Test1", "Test1"},
                { "Test2", "Test2"},
            };
        IReadOnlyDictionary<string, object> readonlyDict = new ReadOnlyDictionary<string, object>(dict);

        try
        {
            const bool isHealthy = true;

            if (isHealthy)
                return Task.FromResult(HealthCheckResult.Healthy("SampleHealthCheck is healthy.", readonlyDict));


            return Task.FromResult(HealthCheckResult.Unhealthy("SampleHealthCheck is Unhealthy.", null, readonlyDict));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("SampleHealthCheck is Unhealthy.", ex, readonlyDict));
        }
    }
}