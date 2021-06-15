using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using Tbd.Shared.Options;

namespace Tbd.WebApi.Shared.CorrelationId
{
    /// <summary>
    /// Middleware which attempts to reads / creates a Correlation ID that can then be used in logs and
    /// passed to upstream requests.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private CorrelationIdOptionsModel _options;

        /// <summary>
        /// Creates a new instance of the CorrelationIdMiddleware.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        public CorrelationIdMiddleware(RequestDelegate next) =>
            _next = next ?? throw new ArgumentNullException(nameof(next));

        /// <summary>
        /// Processes a request to synchronize TraceIdentifier and Correlation ID headers.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="options">The configuration options.</param>
        /// <returns>Task</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task Invoke(HttpContext context, IOptionsSnapshot<CorrelationIdOptionsModel> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));

            var correlationId = SetCorrelationId(context);
            if (_options.UpdateTraceIdentifier)
                context.TraceIdentifier = correlationId;

            if (_options.IncludeInResponse)
            {
                // apply the correlation ID to the response header for client side tracking
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(_options.Header))
                        context.Response.Headers.Add(_options.Header, correlationId);
                    return Task.CompletedTask;
                });
            }

            return _next(context);
        }

        private StringValues SetCorrelationId(HttpContext context)
        {
            var correlationIdFoundInRequestHeader = context.Request.Headers.TryGetValue(_options.Header, out var correlationId);
            if (RequiresGenerationOfCorrelationId(correlationIdFoundInRequestHeader, correlationId))
                correlationId = GenerateCorrelationId(context.TraceIdentifier);

            return correlationId;
        }

        private static bool RequiresGenerationOfCorrelationId(bool idInHeader, StringValues idFromHeader) =>
            !idInHeader || StringValues.IsNullOrEmpty(idFromHeader);

        private StringValues GenerateCorrelationId(string traceIdentifier)
        {
            return _options.UseGuidForCorrelationId || string.IsNullOrWhiteSpace(traceIdentifier)
                ? Guid.NewGuid().ToString()
                : traceIdentifier;
        }
    }
}
