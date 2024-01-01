using System.Runtime.CompilerServices;

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

    public IBaseViewModel AddUnhandledException(Exception ex, ILogger logger, [CallerMemberName] string callerMemberName = null, params KeyValuePair<string, string>[] additionalData)
    {
        if (logger is not null)
            logger.LogError(ex, "{methodName} encountered exception:", callerMemberName);

        // TODO:  verify GetObjectAndMemberName returns name of sub class not base class
        var fullSourceName = this.GetObjectAndMemberName(callerMemberName);

        // TODO:  check HostEnvironment.IsProduction() to redact (remove stack trace)
        var message = $"Exception encountered: {ex}{Environment.NewLine}StackTrace: {ex.StackTrace}";

        ApiResultMessages?.Add(new ApiResultMessageModel
        {
            MessageType = ApiResultMessageModelTypeEnumeration.UnhandledException,
            Code = 600,
            Source = fullSourceName,
            Message = message
        });


        return this;
        /*
    public ISessionViewModel HandleException(Exception ex
        , List<IApiResultMessageModel> apiResultMessages = null
        , string callerComponentName = null
        , [CallerMemberName] string callerMemberName = null
        , params KeyValuePair<string, string>[] additionalData)
    {
        const string NewLineHtml = "<br />";

        //if (ex is AccessTokenNotAvailableException accessTokenNotAvailableException)
        //{
        //    accessTokenNotAvailableException.Redirect();
        //    return;
        //}

        var dictionary = new Dictionary<string, string>(additionalData)
        {
            { "DateTimeOffset", DateTimeOffset.Now.ToString() },
            { "SessionId", SessionId.ToString() },
            { "TraceId", TraceId.ToString() }
        };

        var dictionaryAsString = dictionary?.Count > 0
            ? $"{NewLineHtml}Additional Data: {NewLineHtml}{string.Join(NewLineHtml, dictionary.Select(x => $"{x.Key} = {x.Value}"))}"
            : string.Empty;

        var fullSourceName = $"{callerComponentName}.{callerMemberName}";

        var message = HostEnvironment.IsProduction()
            ? "Oops, we encountered an unhandled exception!  Please try again in a few minutes."
            : $"{fullSourceName} encountered exception: {ex}{NewLineHtml}StackTrace: {ex.StackTrace}{dictionaryAsString}".Replace(Environment.NewLine, NewLineHtml);

        if (HostEnvironment.IsProduction().Equals(false))
            _logger.LogError(ex, $"{fullSourceName}{NewLineHtml}{message}".Replace(NewLineHtml, Environment.NewLine));

        apiResultMessages?.Add(new ApiResultMessageModel
        {
            MessageType = ApiResultMessageModelTypeEnumeration.UnhandledException,
            Code = 600,
            Source = fullSourceName,
            Message = message
        });

        return this;
    }

         */
    }

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

    IBaseViewModel AddUnhandledException(Exception ex, ILogger logger, [CallerMemberName] string callerMemberName = null, params KeyValuePair<string, string>[] additionalData);

    IBaseViewModel AddMessages(IEnumerable<ApiResultMessageModel> messages);

    IBaseViewModel ResetForNextOperation(bool isbusy = true, bool sendIsBusyValueChangedMessage = true);

    IBaseViewModel SetIsBusy(bool isbusy = true, bool sendIsBusyValueChangedMessage = true);

    IBaseViewModel RaisePropertyChangedFor(params string[] propertyNames);
}