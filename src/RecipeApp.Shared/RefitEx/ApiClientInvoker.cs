namespace RecipeApp.Shared.RefitEx;

public class ApiClientInvoker : IApiClientInvoker
{
    protected readonly IHostEnvironment _hostEnvironment;
    protected readonly ILogger<ApiClientInvoker> _logger;

    public ApiClientInvoker(IHostEnvironment hostEnvironment, ILogger<ApiClientInvoker> logger)
    {
        _hostEnvironment = hostEnvironment;
        _logger = logger;
        // TODO RRS:  inject TelemetryClient for AppInsights or cover object for both after reviewing AppInsights POC
    }

    public async Task<(TResult Data, ProblemDetails? Problems)> TryInvokeAsync<TResult>(Func<Task<TResult>> func)
    {
        TResult result = default!;
        ProblemDetails? problemDetails = null;
        Delegate firstDelegate = null!;

        try
        {
            firstDelegate = func.GetInvocationList()[0];

            var t = firstDelegate.Target.ToString();
            var m = firstDelegate.Method.Name;

            _logger.LogInformation("Invoking {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            result = await func.Invoke();
        }
        catch (ApiException apiException)
        {
            _logger.LogError(apiException, "Unhandled exception occurred: {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            problemDetails = await apiException.ToProblemDetailsAsync(_hostEnvironment);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred: {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            problemDetails = exception.ToProblemDetails(_hostEnvironment);
        }

        return (result, problemDetails);
    }

    //  We need a second method for Api's that dont return any data... 204 - No Content
    public async Task<ProblemDetails?> TryInvokeAsync(Func<Task> func)
    {
        ProblemDetails? problemDetails = null;
        Delegate firstDelegate = null!;

        try
        {
            firstDelegate = func.GetInvocationList()[0];
            _logger.LogInformation("Invoking {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            await func.Invoke();
        }
        catch (ApiException apiException)
        {
            _logger.LogError(apiException, "Unhandled exception occurred: {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            problemDetails = await apiException.ToProblemDetailsAsync(_hostEnvironment);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred: {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            problemDetails = exception.ToProblemDetails(_hostEnvironment);
        }

        return problemDetails;
    }
}

public interface IApiClientInvoker
{
    Task<(TResult Data, ProblemDetails? Problems)> TryInvokeAsync<TResult>(Func<Task<TResult>> func);

    Task<ProblemDetails?> TryInvokeAsync(Func<Task> func);
}
