namespace RecipeApp.CoreApi.Features.HealthChecks;

/// <summary>
/// https://www.youtube.com/watch?v=p2faw9DCSsY
/// Note: this is just an example of custom health check, but is not used in favor of AspNetCore.HealthChecks.SqlServer
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly DatabaseHealthCheckRepository _databaseHealthCheckRepository;

    public DatabaseHealthCheck(DatabaseHealthCheckRepository databaseHealthCheckRepository) =>
        _databaseHealthCheckRepository = databaseHealthCheckRepository;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _databaseHealthCheckRepository.CheckDatabaseAsync(cancellationToken);

            return HealthCheckResult.Healthy("DatabaseHealthCheck is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("DatabaseHealthCheck is Unhealthy.", ex);
        }
    }
}
