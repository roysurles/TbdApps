namespace RecipeApp.Shared.RefitEx;

public class ApiClientInvokerEx<TApiClient> : IApiClientInvokerEx<TApiClient> where TApiClient : class
{
    protected readonly TApiClient _apiClient;
    protected readonly IHostEnvironment _hostEnvironment;
    protected readonly ILogger<ApiClientInvokerEx<TApiClient>> _logger;

    public ApiClientInvokerEx(IServiceProvider serviceProvider, IHostEnvironment hostEnvironment, ILogger<ApiClientInvokerEx<TApiClient>> logger)
    {
        _apiClient = serviceProvider.GetRequiredService<TApiClient>();
        _hostEnvironment = hostEnvironment;
        _logger = logger;
    }

    public async Task<(TResult Data, ProblemDetails Problems)> TryInvokeAsync<TResult>(Expression<Func<TApiClient, Task<TResult>>> expression)
    {
        TResult result = default!;
        ProblemDetails problemDetails = null;
        Delegate firstDelegate = null!;

        try
        {
            var p = expression.Parameters;

            var func = expression.Compile();
            firstDelegate = func.GetInvocationList()[0];

            // ApiClientInvokerEx

            var m3 = expression.Name;
            var t3 = expression.Parameters[0].ToString();
            var b = expression.Body.ToString();             /// this by itself seems best

            var m = firstDelegate.Method.Name;
            var t = firstDelegate.Target.ToString();

            var m2 = func.Method.Name;
            var t2 = func.Target.ToString();


            _logger.LogInformation("Invoking {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            result = await func.Invoke(_apiClient);
        }
        catch (ApiException apiException)
        {
            _logger.LogError(apiException, "Unhandled exception occurred: {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            problemDetails = await apiException.ToProblemDetailsAsync(null);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred: {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            problemDetails = exception.ToProblemDetails(null);
        }

        return (result, problemDetails);
    }

    //  We need a second method for Api's that dont return any data... 204 - No Content
    public async Task<ProblemDetails> TryInvokeAsync(Expression<Func<TApiClient, Task>> expression)
    {
        ProblemDetails problemDetails = null;
        Delegate firstDelegate = null!;

        try
        {
            var func = expression.Compile();
            firstDelegate = func.GetInvocationList()[0];

            _logger.LogInformation("Invoking {Target} : {MethodName}", firstDelegate.Target, firstDelegate.Method.Name);
            // TODO RRS:  Invoke TelemetryClient for AppInsights
            await func.Invoke(_apiClient);
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

public interface IApiClientInvokerEx<TApiClient>
{
    Task<(TResult Data, ProblemDetails Problems)> TryInvokeAsync<TResult>(Expression<Func<TApiClient, Task<TResult>>> expression);

    Task<ProblemDetails> TryInvokeAsync(Expression<Func<TApiClient, Task>> expression);
}
