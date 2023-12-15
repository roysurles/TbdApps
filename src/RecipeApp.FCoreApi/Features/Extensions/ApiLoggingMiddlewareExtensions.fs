namespace RecipeApp.FCoreApi.Features.Extensions

open Microsoft.AspNetCore.Builder

open Tbd.Shared.ApiLog

open Tbd.WebApi.Shared.ApiLogging

module ApiLoggingMiddlewareExtensions =
    type IApplicationBuilder with
        member this.UseApiLoggingEx() =
            this.UseMiddleware<ApiLoggingMiddleware>() |> ignore
            this

    type IApplicationBuilder with
        member this.UseApiLoggingEx(apiLoggingOptionsModel: ApiLoggingOptionsModel) =
            if apiLoggingOptionsModel = null
            then
                nullArg "apiLoggingOptionsModel"

            this.UseMiddleware<ApiLoggingMiddleware>(apiLoggingOptionsModel) |> ignore
            this