namespace RecipeApp.Shared.RefitEx;

public static class ApiClient
{
    public static async Task<(TResult Data, ProblemDetails? Problems)> TryInvokeAsync<TResult>(Func<Task<TResult>> func)
    {
        TResult result = default!;
        ProblemDetails? problemDetails = null;

        try
        {
            result = await func.Invoke();
        }
        catch (ApiException apiException)
        {
            problemDetails = await apiException.ToProblemDetailsAsync(null);
        }
        catch (Exception exception)
        {
            problemDetails = exception.ToProblemDetails(null);
        }

        return (result, problemDetails);
    }

    public static async Task<ProblemDetails?> TryInvokeAsync(Func<Task> func)
    {
        ProblemDetails? problemDetails = null;

        try
        {
            await func.Invoke();
        }
        catch (ApiException apiException)
        {
            problemDetails = await apiException.ToProblemDetailsAsync(null);
        }
        catch (Exception exception)
        {
            problemDetails = exception.ToProblemDetails(null);
        }

        return problemDetails;
    }

}
