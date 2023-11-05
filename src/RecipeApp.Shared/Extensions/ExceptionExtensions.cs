namespace RecipeApp.Shared.Extensions;

public static class ExceptionExtensions
{
    public static async Task<ProblemDetails> ToProblemDetailsAsync(this ApiException apiException, IHostEnvironment? hostEnvironment)
    {
        ProblemDetails problemDetails;

        try
        {
            // first, try to deserialize as ProblemDetails if the server returned ProblemDetails
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            problemDetails = await apiException.GetContentAsAsync<ProblemDetails>();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        catch (Exception)
        {
            // if we cant deserialize as ProblemDetails, then create our own ProblemDetails
            problemDetails = apiException.ToProblemDetails(hostEnvironment);
        }

        return problemDetails!;
    }

    public static ProblemDetails ToProblemDetails(this ApiException apiException, IHostEnvironment? hostEnvironment)
    {
        return new ProblemDetails
        {
            Title = $"Unhandled exception: {apiException.GetType().Name}",
            Status = (int)apiException.StatusCode,
            Detail = apiException.GetExceptionMessage(hostEnvironment)
        };
    }

    public static ProblemDetails ToProblemDetails(this Exception exception, IHostEnvironment? hostEnvironment)
    {
        return new ProblemDetails
        {
            Title = $"Unhandled exception: {exception.GetType().Name}",
            Status = 600,       // Note:  600 status code here, because we had some issue beyond what we expected... not really a 500
            Detail = exception.GetExceptionMessage(hostEnvironment)
        };
    }

    public static string GetExceptionMessage(this Exception exception, IHostEnvironment? hostEnvironment)
    {
        // Redact exception data for Production
        return hostEnvironment?.IsProduction() == true
            ? exception.Message
            : exception.ToString();
    }

}
