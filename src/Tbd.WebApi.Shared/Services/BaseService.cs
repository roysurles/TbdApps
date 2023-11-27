namespace Tbd.WebApi.Shared.Services;

public abstract class BaseService
{
    protected readonly IServiceProvider _serviceProvider;

    protected BaseService(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    protected virtual IApiResultModel<T> CreateApiResultModel<T>() =>
        _serviceProvider.GetRequiredService<IApiResultModel<T>>();
}
