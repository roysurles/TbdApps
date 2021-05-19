using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

using Tbd.Shared.ApiLog;

namespace Tbd.WebApi.Shared.Extensions
{
    public static class HttpContextExtensions
    {
        public static ApiLogDto InitializeApiLogDto(this HttpContext httpContext)
        {
            var queryString = httpContext.Request.QueryString.HasValue ? $"/{httpContext.Request.QueryString}" : "";
            var (ControllerName, ActionName) = httpContext.GetControllerAndActionNameFromEndpoint();

            return new ApiLogDto
            {
                ConnectionId = httpContext.Connection.Id,
                TraceId = httpContext.TraceIdentifier,
                Claims = httpContext.User.Claims?.Any() == true ? JsonSerializer.Serialize(httpContext.User.Claims.Select(claim => new { claim.Type, claim.Value }), new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }) : null,
                LocalIpAddress = httpContext.Connection.LocalIpAddress?.ToString(),
                RemoteIpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
                AssemblyName = Assembly.GetEntryAssembly().GetName().FullName,
                Url = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{queryString}",
                ControllerName = ControllerName,
                ActionName = ActionName,
                HttpProtocol = httpContext.Request.Protocol,
                HttpMethod = httpContext.Request.Method,
                HttpStatusCode = httpContext.Response.StatusCode
            };
        }

        public static (string ControllerName, string ActionName) GetControllerAndActionNameFromEndpoint(this HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint == null)
                return (null, null);

            var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

            return controllerActionDescriptor == null
                ? (null, null)
                : (controllerActionDescriptor.ControllerName, controllerActionDescriptor.ActionName);
        }

        public static async Task<string> GetHttpRequestBodyAsStringAsync(this HttpContext httpContext)
        {
            var request = httpContext.Request;
            request.EnableBuffering();
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
            var requestBodyAsString = await reader.ReadToEndAsync().ConfigureAwait(false);
            request.Body.Position = 0;

            return requestBodyAsString;
        }

        public static async Task<string> GetHttpResponseBodyAsStringAsync(this HttpContext httpContext)
        {
            var httpResponse = httpContext.Response;
            httpResponse.Body.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[httpResponse.Body.Length];
            _ = await httpResponse.Body.ReadAsync(buffer.AsMemory(0, Convert.ToInt32(httpResponse.Body.Length))).ConfigureAwait(false);
            var responseBodyAsString = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            httpResponse.Body.Seek(0, SeekOrigin.Begin);

            return responseBodyAsString;
        }

        public static Exception GetExceptionFromIExceptionHandlerFeature(this HttpContext httpContext) =>
            httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
    }
}
