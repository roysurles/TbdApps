using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

using Tbd.Shared.Options;
using Tbd.WebApi.Shared.Middleware;

namespace Tbd.WebApi.Shared.Extensions
{
    /// <summary>
    /// CorrelationId Extensions
    /// </summary>
    public static class CorrelationIdMiddlewareExtensions
    {
        /// <summary>
        /// UseCorrelationId
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder UseCorrelationIdEx(this IApplicationBuilder app) =>
            app.UseMiddleware<CorrelationIdMiddleware>();

        /// <summary>
        /// UseCorrelationId
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="header">string</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder UseCorrelationIdEx(this IApplicationBuilder app, string header) =>
            app.UseCorrelationIdEx(new CorrelationIdOptionsModel { Header = header });

        /// <summary>
        /// UseCorrelationId
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="correlationIdOptionsModel">CorrelationIdSettingsModel</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder UseCorrelationIdEx(this IApplicationBuilder app, CorrelationIdOptionsModel correlationIdOptionsModel)
        {
            if (correlationIdOptionsModel is null)
                throw new ArgumentNullException(nameof(correlationIdOptionsModel));

            return app.UseMiddleware<CorrelationIdMiddleware>(Options.Create(correlationIdOptionsModel));
        }
    }
}
