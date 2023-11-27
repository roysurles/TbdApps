namespace Tbd.WebApi.Shared.Handlers;

/// <summary>
/// https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-web-api-call-api-call-api?tabs=aspnetcore
/// </summary>
public class CustomDelegatingHandler : DelegatingHandler
{
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly HttpContext _httpContext;
    protected readonly ILogger<CustomDelegatingHandler> _logger;

    public CustomDelegatingHandler(IHttpContextAccessor httpContextAccessor
        , ILogger<CustomDelegatingHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpContext = _httpContextAccessor.HttpContext;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _httpContext.GetTokenAsync("access_token");
        if (string.IsNullOrWhiteSpace(accessToken).Equals(false))
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

        var traceIdentifier = _httpContext.TraceIdentifier;
        var headers = _httpContext.Request.Headers;
        if (headers.ContainsKey("X-Correlation-ID") && !string.IsNullOrWhiteSpace(headers["X-Correlation-ID"]))
            traceIdentifier = headers["X-Correlation-ID"].ToString();

        request.Headers.Remove("X-Correlation-ID");
        request.Headers.Add("X-Correlation-ID", traceIdentifier);

        return await base.SendAsync(request, cancellationToken);
    }
}
