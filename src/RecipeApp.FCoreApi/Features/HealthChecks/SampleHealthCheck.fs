namespace RecipeApp.FCoreApi.Features.HealthChecks

open System.Collections.Generic
open System.Collections.ObjectModel
open System.Threading

open Microsoft.Extensions.Diagnostics.HealthChecks

type SampleHealthCheck() =
    interface IHealthCheck with
        member this.CheckHealthAsync(context: HealthCheckContext , cancellationToken: CancellationToken) =
            async {

                let dict = new Dictionary<string, System.Object>()
                dict.Add("Test1", "Test1")
                dict.Add("Test2", "Test2")

                let readonlyDict = new ReadOnlyDictionary<string, System.Object>(dict);

                try
                    let isHealthy = true

                    if isHealthy
                    then
                        return HealthCheckResult.Healthy("SampleHealthCheck is healthy.", readonlyDict)
                    else
                        return HealthCheckResult.Healthy("SampleHealthCheck is Unhealthy.")
                with
                    | ex -> return HealthCheckResult.Unhealthy("SampleHealthCheck is Unhealthy.", ex)
            }
            |> Async.StartAsTask


