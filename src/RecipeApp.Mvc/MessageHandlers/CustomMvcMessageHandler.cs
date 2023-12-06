namespace RecipeApp.Mvc.MessageHandlers;

// TODO: See if we can use the Blazor shared CustomMessageHandler w/ the SessionViewModel
public class CustomMvcMessageHandler : DelegatingHandler
{
    protected readonly IHttpContextAccessor _httpContextAccessor;

    public CustomMvcMessageHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Remove("X-Correlation-ID");
        request.Headers.Add("X-Correlation-ID", _httpContextAccessor.HttpContext?.TraceIdentifier);
        return base.SendAsync(request, cancellationToken);
    }
}
