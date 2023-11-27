namespace RecipeApp.Shared.Extensions;

/// <summary>
/// RequestServicesExtensions
/// </summary>
public static class RequestServicesExtensions
{
    /// <summary>
    /// GetRequiredService
    /// </summary>
    /// <typeparam name="T">Type of desired service.</typeparam>
    /// <param name="controller">ControllerBase</param>
    /// <returns>Type of desired service.</returns>
    public static T GetRequiredService<T>(this ControllerBase controller) =>
        controller.HttpContext.RequestServices.GetRequiredService<T>();

    /// <summary>
    /// GetRequiredService
    /// </summary>
    /// <typeparam name="T">Type of desired service.</typeparam>
    /// <param name="httpContext">HttpContext</param>
    /// <returns>Type of desired service.</returns>
    public static T GetRequiredService<T>(this HttpContext httpContext) =>
        httpContext.RequestServices.GetRequiredService<T>();

    /// <summary>
    /// GetRequiredService
    /// </summary>
    /// <typeparam name="T">Type of desired service.</typeparam>
    /// <param name="actionExecutingContext">ActionExecutingContext</param>
    /// <returns>Type of desired service.</returns>
    public static T GetRequiredService<T>(this ActionExecutingContext actionExecutingContext) =>
        actionExecutingContext.HttpContext.RequestServices.GetRequiredService<T>();
}
