namespace RecipeApp.FCoreApi.Features.Extensions

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.DependencyInjection

module ApiVersioningMiddlewareExtensions =
    type IServiceCollection with
        member this.AddApiVersioningEx() =
            this.AddApiVersioning(fun options ->
                options.ReportApiVersions <- true
                options.AssumeDefaultVersionWhenUnspecified <- true
                options.DefaultApiVersion <- new ApiVersion(majorVersion = 1, minorVersion = 0))
            |> ignore
            this
