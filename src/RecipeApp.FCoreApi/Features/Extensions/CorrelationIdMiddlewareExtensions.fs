namespace RecipeApp.FCoreApi.Features.Extensions

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Options

open Tbd.Shared.Options

open Tbd.WebApi.Shared.CorrelationId

module CorrelationIdMiddlewareExtensions =
    type IApplicationBuilder with
        member this.UseCorrelationIdEx() =
            this.UseMiddleware<CorrelationIdMiddleware>() |> ignore
            this

    type IApplicationBuilder with
        member this.UseCorrelationIdEx(header: string) =
            this.UseCorrelationIdEx(new CorrelationIdOptionsModel (Header = header)) |> ignore
            this

    type IApplicationBuilder with
        member this.UseCorrelationIdEx(correlationIdOptionsModel: CorrelationIdOptionsModel) =
            if correlationIdOptionsModel = null
            then
                nullArg "correlationIdOptionsModel"

            this.UseMiddleware<CorrelationIdMiddleware>(Options.Create(correlationIdOptionsModel)) |> ignore
            this
