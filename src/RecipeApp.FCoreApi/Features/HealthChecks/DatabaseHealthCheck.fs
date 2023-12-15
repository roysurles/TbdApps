namespace RecipeApp.FCoreApi.Features.HealthChecks

open System.Threading

open Microsoft.Extensions.Diagnostics.HealthChecks

open Tbd.WebApi.Shared.Repositories

type DatabaseHealthCheck(databaseHealthCheckRepository: DatabaseHealthCheckRepository) =
    interface IHealthCheck with
        member this.CheckHealthAsync(context: HealthCheckContext , cancellationToken: CancellationToken) =
            async {

                try
                    do! databaseHealthCheckRepository.CheckDatabaseAsync(cancellationToken) |> Async.AwaitTask

                    return HealthCheckResult.Healthy("DatabaseHealthCheck is healthy.")
                with
                    | ex -> return HealthCheckResult.Unhealthy("DatabaseHealthCheck is Unhealthy.", ex)
            }
            |> Async.StartAsTask
