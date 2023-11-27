namespace Tbd.WebApi.Shared.ApiLogging;

public class ApiLoggingMiddleware
{
    protected readonly RequestDelegate _next;
    protected readonly ILogger<ApiLoggingMiddleware> _logger;
    protected readonly ILogger<ApiLogRepository> _apiLogRepositoryLogger;
    protected ApiLoggingOptionsModel _options;

    public ApiLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<ApiLoggingMiddleware>();
        _apiLogRepositoryLogger = loggerFactory.CreateLogger<ApiLogRepository>();
    }

    public async Task Invoke(HttpContext httpContext, IOptionsSnapshot<ApiLoggingOptionsModel> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));

        if (_options.IsEnabled && !IsFullUrlExcluded(httpContext))
            await InvokeWithLoggingAsync(httpContext).ConfigureAwait(false);
        else
            await _next(httpContext).ConfigureAwait(false);
    }

    protected bool IsFullUrlExcluded(HttpContext httpContext)
    {
        var fullRequestUrl = httpContext.GetFullUrl();
        foreach (var excludedUrl in _options.ExcludedUrls)
        {
            if ((excludedUrl.EndsWith('*') && fullRequestUrl.StartsWith(excludedUrl.Trim().Replace("*", ""), StringComparison.OrdinalIgnoreCase)) || (!excludedUrl.EndsWith('*') && string.Equals(fullRequestUrl, excludedUrl, StringComparison.OrdinalIgnoreCase)))
                return true;
        }

        return false;
    }

    protected async Task InvokeWithLoggingAsync(HttpContext httpContext)
    {
        Stopwatch loggingStopWatch = null;
        Stopwatch apiActionStopWatch = null;
        ApiLogDto apiLogDto = null;
        int? httpStatusCode = null;
        string exceptionData = null;

        try
        {
            loggingStopWatch = Stopwatch.StartNew();

            apiLogDto = httpContext.InitializeApiLogDto();

            var originalReponseBodyStream = httpContext.Response.Body;
            using var responseBody = new MemoryStream();
            httpContext.Response.Body = responseBody;

            apiLogDto.HttpRequestBody = await httpContext.GetHttpRequestBodyAsStringAsync().ConfigureAwait(false);

            loggingStopWatch.Stop();
            apiActionStopWatch = Stopwatch.StartNew();
            await _next(httpContext).ConfigureAwait(false);
            apiActionStopWatch.Stop();
            loggingStopWatch.Start();

            apiLogDto.HttpResponseBody = await httpContext.GetHttpResponseBodyAsStringAsync().ConfigureAwait(false);
            await responseBody.CopyToAsync(originalReponseBodyStream).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // TODO:  need similar logic as ExceptionHandler Middleware here in case ExceptionHandler Middleware is NOT used
            httpStatusCode = (int)HttpStatusCode.InternalServerError;
            exceptionData = ex.GetExceptionData();
            throw;
        }
        finally
        {
            apiLogDto.ElapsedMilliseconds = apiActionStopWatch.ElapsedMilliseconds;
            apiLogDto.HttpStatusCode = httpStatusCode ?? httpContext.Response.StatusCode;
            apiLogDto.ExceptionData = exceptionData ?? httpContext.GetExceptionFromIExceptionHandlerFeature()?.GetExceptionData();

            if (apiLogDto.HttpStatusCode >= _options.MinimumHttpStatusCode)
                _ = await ApiLogRepository.InsertApiLogDtoAsync(apiLogDto, _apiLogRepositoryLogger, _options.ConnectionString).ConfigureAwait(false);

            loggingStopWatch.Stop();
            _logger.LogInformation("Api Action Elapsed Milliseconds: {0}", apiActionStopWatch.ElapsedMilliseconds);
            _logger.LogInformation("Api Logging Elapsed Milliseconds: {0}", loggingStopWatch.ElapsedMilliseconds);
        }
    }
}
