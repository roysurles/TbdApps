namespace RecipeApp.Maui.Features.Shared.Models;

public partial class BaseViewModel : ObservableValidator, IBaseViewModel
{
    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public bool isBusy;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public ObservableCollection<IApiResultMessageModel> apiResultMessages = new();

    public IBaseViewModel ClearApiResultMessages()
    {
        ApiResultMessages.Clear();
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

    public IBaseViewModel ResetForNextOperation(bool isbusy = true, bool sendIsBusyValueChangedMessage = true)
    {
        IsBusy = isbusy;
        if (sendIsBusyValueChangedMessage)
            WeakReferenceMessenger.Default.Send(new IsBusyValueChangedMessage(IsBusy));

        ClearApiResultMessages();

        return this;
    }

    public IBaseViewModel SetIsBusy(bool isbusy = true, bool sendIsBusyValueChangedMessage = true)
    {
        IsBusy = isbusy;
        if (sendIsBusyValueChangedMessage)
            WeakReferenceMessenger.Default.Send(new IsBusyValueChangedMessage(IsBusy));

        return this;
    }

    public IBaseViewModel RaisePropertyChangedFor(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
            OnPropertyChanged(propertyName);

        return this;
    }
}

public interface IBaseViewModel
{
    bool IsBusy { get; set; }

    ObservableCollection<IApiResultMessageModel> ApiResultMessages { get; set; }

    IBaseViewModel ClearApiResultMessages();

    IBaseViewModel AddApiResultMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, int? code = null);

    IBaseViewModel AddInformationMessage(string message, string source = null, int? code = null);

    IBaseViewModel AddWarningMessage(string message, string source = null, int? code = null);

    IBaseViewModel AddErrorMessage(string message, string source = null, int? code = null);

    IBaseViewModel AddMessages(IEnumerable<ApiResultMessageModel> messages);

    IBaseViewModel ResetForNextOperation(bool isbusy = true, bool sendIsBusyValueChangedMessage = true);

    IBaseViewModel SetIsBusy(bool isbusy = true, bool sendIsBusyValueChangedMessage = true);

    IBaseViewModel RaisePropertyChangedFor(params string[] propertyNames);
}