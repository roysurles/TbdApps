using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Services;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.MessageHandlers
{
    public class CustomMessageHandler : DelegatingHandler
    {
        protected readonly ISessionService _sessionService;

        public CustomMessageHandler(ISessionService sessionService) =>
            _sessionService = sessionService;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Remove("X-Correlation-ID");
            request.Headers.Add("X-Correlation-ID", _sessionService.TraceId.ToString());
            return base.SendAsync(request, cancellationToken);
        }
    }
}
