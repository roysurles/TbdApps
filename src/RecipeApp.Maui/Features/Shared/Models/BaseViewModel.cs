namespace RecipeApp.Maui.Features.Shared.Models;

public partial class BaseViewModel : ObservableValidator, IBaseViewModel
{
    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public bool isBusy;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public ObservableCollection<IApiResultMessageModel> apiResultMessages = new();

    protected IBaseViewModel ClearApiResultMessages()
    {
        ApiResultMessages.Clear();
        return this;
    }

    protected IBaseViewModel AddApiResultMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, int? code = null)
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

    protected IBaseViewModel AddInformationMessage(string message, string source = null, int? code = null) =>
        AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Information, message, source, code);

    protected IBaseViewModel AddWarningMessage(string message, string source = null, int? code = null) =>
        AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Warning, message, source, code);

    protected IBaseViewModel AddErrorMessage(string message, string source = null, int? code = null) =>
        AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Error, message, source, code);

    protected IBaseViewModel AddMessages(IEnumerable<ApiResultMessageModel> messages)
    {
        ApiResultMessages.AddRange(messages);
        return this;
    }

    protected IBaseViewModel ResetForNextOperation(bool isbusy = true)
    {
        IsBusy = isbusy;
        ClearApiResultMessages();

        return this;
    }
}

public interface IBaseViewModel
{
    bool IsBusy { get; set; }

    ObservableCollection<IApiResultMessageModel> ApiResultMessages { get; set; }
}