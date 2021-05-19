
using System;

using Microsoft.AspNetCore.Builder;

using Tbd.Shared.Options;
using Tbd.WebApi.Shared.Middleware;

namespace Tbd.WebApi.Shared.Extensions
{
    public static class ApiLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiLoggingEx(this IApplicationBuilder app) =>
            app.UseMiddleware<ApiLoggingMiddleware>();

        public static IApplicationBuilder UseApiLoggingEx(this IApplicationBuilder app, ApiLoggingOptionsModel apiLoggingOptionsModel)
        {
            if (apiLoggingOptionsModel is null)
                throw new ArgumentNullException(nameof(apiLoggingOptionsModel));

            return app.UseMiddleware<ApiLoggingMiddleware>(apiLoggingOptionsModel);
        }
    }
}
