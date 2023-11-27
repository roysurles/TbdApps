namespace RecipeApp.Shared.Models;

public class BaseViewModel : IBaseViewModel
{
    public bool IsLoading { get; set; }

    public bool IsBusy { get; set; }

    public List<IApiResultMessageModel> ApiResultMessages { get; set; } = new List<IApiResultMessageModel>();

    public IBaseViewModel SetIsLoadingFlag(bool isLoading, Action workerItem)
    {
        IsLoading = isLoading;
        workerItem.Invoke();

        return this;
    }

    public async Task<IBaseViewModel> SetIsLoadingFlagAsync(bool isLoading, EventCallback eventCallback)
    {
        IsLoading = isLoading;
        await eventCallback.InvokeAsync();

        return this;
    }

    public async Task<IBaseViewModel> SetIsLoadingFlagAsync<T>(bool isLoading, EventCallback<T> eventCallback, T arg)
    {
        IsLoading = isLoading;
        await eventCallback.InvokeAsync(arg);

        return this;
    }

    public IBaseViewModel SetIsBusyFlag(bool isBusy, Action workerItem)
    {
        IsBusy = isBusy;
        workerItem.Invoke();

        return this;
    }

    public async Task<IBaseViewModel> SetIsBusyFlagAsync(bool isBusy, EventCallback eventCallback)
    {
        IsBusy = isBusy;
        await eventCallback.InvokeAsync();

        return this;
    }

    public async Task<IBaseViewModel> SetIsBusyFlagAsync<T>(bool isBusy, EventCallback<T> eventCallback, T arg)
    {
        IsBusy = isBusy;
        await eventCallback.InvokeAsync(arg);

        return this;
    }

    public IBaseViewModel ClearApiResultMessages()
    {
        ApiResultMessages = new List<IApiResultMessageModel>();
        return this;
    }

    public IBaseViewModel AddApiResultMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, int? code = null)
    {
        ApiResultMessages.Add(new ApiResultMessageModel
        {
            MessageType = apiResultMessageType,
            Message = message,
            Source = source,
            Code = code
        });
        return this;
    }

    public IBaseViewModel AddInformationMessage(string message, string source = null, int? code = null) =>
        AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Information, message, source, code);

    public IBaseViewModel AddWarningMessage(string message, string source = null, int? code = null) =>
        AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Warning, message, source, code);

    public IBaseViewModel AddErrorMessage(string message, string source = null, int? code = null) =>
        AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Error, message, source, code);

    public IBaseViewModel AddMessages(IEnumerable<ApiResultMessageModel> messages)
    {
        ApiResultMessages.AddRange(messages);
        return this;
    }
}

public interface IBaseViewModel
{
    bool IsLoading { get; set; }

    bool IsBusy { get; set; }

    List<IApiResultMessageModel> ApiResultMessages { get; set; }

    IBaseViewModel SetIsLoadingFlag(bool isLoading, Action workerItem);

    Task<IBaseViewModel> SetIsLoadingFlagAsync(bool isLoading, EventCallback eventCallback);

    Task<IBaseViewModel> SetIsLoadingFlagAsync<T>(bool isLoading, EventCallback<T> eventCallback, T arg);

    IBaseViewModel SetIsBusyFlag(bool isBusy, Action workerItem);

    Task<IBaseViewModel> SetIsBusyFlagAsync(bool isBusy, EventCallback eventCallback);

    Task<IBaseViewModel> SetIsBusyFlagAsync<T>(bool isBusy, EventCallback<T> eventCallback, T arg);

    IBaseViewModel ClearApiResultMessages();

    IBaseViewModel AddApiResultMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, int? code = null);

    IBaseViewModel AddInformationMessage(string message, string source = null, int? code = null);

    IBaseViewModel AddWarningMessage(string message, string source = null, int? code = null);

    IBaseViewModel AddErrorMessage(string message, string source = null, int? code = null);

    IBaseViewModel AddMessages(IEnumerable<ApiResultMessageModel> messages);
}