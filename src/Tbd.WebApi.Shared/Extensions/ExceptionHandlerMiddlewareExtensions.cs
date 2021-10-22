using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using RecipeApp.Shared.Extensions;

using Tbd.Shared.ApiResult;
using Tbd.Shared.Constants;
using Tbd.Shared.Extensions;

namespace Tbd.WebApi.Shared.Extensions
{
    /// <summary>
    /// ExceptionHandler Extensions
    /// </summary>
    public static class ExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Extension for using custom ExceptionHandler.
        /// There is no need to have this enabled if ApiLoggingMiddleware is enabled, because this will be redundant.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="hostEnvironment">IHostingEnvironment</param>
        public static void UseExceptionHandlerEx(this IApplicationBuilder app, IHostEnvironment hostEnvironment, bool isLoggingEnabled)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exception = context.GetExceptionFromIExceptionHandlerFeature();
                    if (exception is null)
                        return;

                    if (exception is TaskCanceledException || exception is OperationCanceledException)       // Handle if needed
                        return;

                    var httpStatusCode = HttpStatusCode.InternalServerError;

                    // Map specific exception to specific HttpStatusCode
                    if (exception is UnauthorizedAccessException)
                        httpStatusCode = HttpStatusCode.Unauthorized;

                    var apiResult = context.GetRequiredService<IApiResultModel<object>>()
                        .SetHttpStatusCode(httpStatusCode);

                    // Redact exception data for Production
                    if (hostEnvironment.IsProduction())
                        apiResult.AddMessage(exception, "We apologize, but an error occurred while processing this request.", context.Request.Path, httpStatusCode);
                    else
                        apiResult.AddMessage(exception, $"An error occurred while processing this request: {exception}", context.Request.Path, httpStatusCode);

                    context.Response.ContentType = ContentTypeConstants.ApplicationJson;
                    context.Response.StatusCode = (int)httpStatusCode;

                    if (isLoggingEnabled)
                    {
                        var apiLogDto = context.InitializeApiLogDto();
                        apiLogDto.HttpRequestBody = await context.GetHttpRequestBodyAsStringAsync().ConfigureAwait(false);
                        apiLogDto.ExceptionData = exception.GetExceptionData();
                    }

                    await context.Response.WriteAsync(JsonSerializer.Serialize(apiResult)).ConfigureAwait(false);
                });
            });
        }
    }
}
