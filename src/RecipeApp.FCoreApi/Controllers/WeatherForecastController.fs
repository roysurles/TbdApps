namespace RecipeApp.FCoreApi.Controllers

open System
open System.Collections.Generic
open System.Data
open System.Data.SqlClient
open System.Linq
open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open RecipeApp.FCoreApi
open RecipeApp.FCoreApi.Features.Introduction.V1_0.Introduction

open RecipeApp.FCoreApi.Features.Helpers
open RecipeApp.FCoreApi.Features.Helpers.DbHelpers
open System.Data.Common

open Dapper


[<ApiController>]
[<Route("[controller]")>]
type WeatherForecastController (logger : ILogger<WeatherForecastController>, repo : IIntroductionRepositoryV1_0) as this  =
    inherit ControllerBase()

    let summaries =
        [|
            "Freezing"
            "Bracing"
            "Chilly"
            "Cool"
            "Mild"
            "Warm"
            "Balmy"
            "Hot"
            "Sweltering"
            "Scorching"
        |]

    member private _.GetCs =
        this.HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("Default")

    [<HttpGet>]
    member _.Get(cancellationToken: CancellationToken) : Task<ActionResult> =
        //let x = 1 / 0

        async {
            //https://dev.to/kspeakman/dirt-simple-sql-queries-in-f-a37
            let config = this.HttpContext.RequestServices.GetRequiredService<IConfiguration>()
            let cs = config.GetConnectionString("Default")
            use! c = ConnectionHelper.CreateOpenSqlConnectionAsync cs cancellationToken
            //let! s = DbHelper.GetSqlConnectionAsync2 "" (new CancellationToken())
            use! t = c.BeginTransactionAsync(cancellationToken).AsTask() |> Async.AwaitTask<DbTransaction>

            // https://nozzlegear.com/blog/using-dapper-with-fsharp
            let inline (=>) a b = a, box b
            let parameters = dict [
                "Id" => Guid.Empty
            ]

            //let parameters = {| Id = Guid.Empty |}
            let commandDefinition = new CommandDefinition(commandText = "IntroductionDelete", parameters = parameters, transaction = t, commandType = CommandType.StoredProcedure, cancellationToken = cancellationToken)

            let! result1 = c.ExecuteScalarAsync<int>(commandDefinition) |> Async.AwaitTask

            t.Commit()

            // **************

            use uow = DbHelpers.SqlUoW(this.GetCs, cancellationToken)

            //use! (c, t) = ConnectionHelper.CreateSqlUnitOfWorkAsync cs cancellationToken

            // **************

            let! result = repo.DeleteAsync (Guid.NewGuid()) cancellationToken |> Async.AwaitTask
            //let rng = System.Random()
            //[|
            //    for index in 0..4 ->
            //        { Date = DateTime.Now.AddDays(float index)
            //          TemperatureC = rng.Next(-20,55)
            //          Summary = summaries.[rng.Next(summaries.Length)] }
            //|]
            return this.Ok(result) :> ActionResult
        }
        |> Async.StartAsTask

    //[<HttpGet>]
    //member _.Get() =
    //    //let x = 1 / 0
    //    let rng = System.Random()
    //    [|
    //        for index in 0..4 ->
    //            { Date = DateTime.Now.AddDays(float index)
    //              TemperatureC = rng.Next(-20,55)
    //              Summary = summaries.[rng.Next(summaries.Length)] }
    //    |]
