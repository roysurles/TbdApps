namespace Tbd.WebApi.Shared.Extensions;

public static class ApiVersioningMiddlewareExtensions
{
    public static void AddApiVersioningEx(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });
    }
}
