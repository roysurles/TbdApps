namespace Tbd.Shared.ApiResult;

public static class ApiResultModelExtensions
{
    public static IApiResultModel<T> CreateApiResultModel<T>(this IServiceProvider serviceProvider) =>
        serviceProvider.GetRequiredService<IApiResultModel<T>>();
}
