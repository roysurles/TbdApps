namespace RecipeApp.FCoreApi.Features.Extensions

open System
open System.Net
open System.Text.Json
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Diagnostics
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting

open Tbd.Shared.ApiResult
open Tbd.Shared.Constants
open Tbd.WebApi.Shared.Extensions
open Tbd.Shared.Extensions

module ExceptionHandlerMiddlewareExtensions =
    type IApplicationBuilder with
        member this.UseExceptionHandlerEx(hostEnvironment: IHostEnvironment , isLoggingEnabled: bool) =
            this.UseExceptionHandler(fun appBuilder ->
                appBuilder.Run(fun (context: HttpContext) ->
                    async {
                        let ex = context.Features.Get<IExceptionHandlerFeature>().Error

                        let mutable httpStatusCode = HttpStatusCode.InternalServerError;

                        match ex with
                        | null -> ignore
                        | TaskCanceledException -> ignore           // Handle if needed
                        | OperationCanceledException  -> ignore     // Handle if needed
                        | UnauthorizedAccessException ->
                            httpStatusCode <- HttpStatusCode.Unauthorized;

                        // TODO F#: let apiResult = context.RequestServices.GetRequiredService<IApiResultModel<System.Object>>()
                        let apiResult = new ApiResultModel<System.Object>()
                        apiResult.SetHttpStatusCode(httpStatusCode) |> ignore

                        // Redact exception data for Production
                        if hostEnvironment.IsProduction()
                        then
                            apiResult.AddMessage(ex, "We apologize, but an error occurred while processing this request.", context.Request.Path, httpStatusCode) |> ignore
                        else
                            apiResult.AddMessage(ex, $"An error occurred while processing this request: {ex}", context.Request.Path, httpStatusCode) |> ignore

                        context.Response.ContentType <- ContentTypeConstants.ApplicationJson;
                        context.Response.StatusCode <- (int)httpStatusCode;

                        if isLoggingEnabled
                        then
                            let apiLogDto = context.InitializeApiLogDto()
                            //apiLogDto.HttpRequestBody <- !context.GetHttpRequestBodyAsStringAsync()
                            apiLogDto.ExceptionData <- ex.GetExceptionData()

                        let json = JsonSerializer.Serialize(apiResult)
                        do! context.Response.WriteAsync(json) |> Async.AwaitTask

                    }
                    |> Async.StartAsTask :> Task
                )
            )
