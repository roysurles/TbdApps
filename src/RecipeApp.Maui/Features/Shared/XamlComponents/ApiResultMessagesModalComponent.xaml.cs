namespace RecipeApp.Maui.Features.Shared.XamlComponents;

public partial class ApiResultMessagesModalComponent : ContentView
{
    public ApiResultMessagesModalComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ApiResultMessagesProperty =
        BindableProperty.Create(nameof(ApiResultMessages), typeof(IList<IApiResultMessageModel>), typeof(ApiResultMessagesModalComponent), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (ApiResultMessagesModalComponent)bindable;

            control.IsVisible = control.ApiResultMessages.Any();

            var categoryCount = 0;
            if (control.HasInformations) categoryCount++;
            if (control.HasWarnings) categoryCount++;
            if (control.HasErrors) categoryCount++;
            if (control.HasUnhandledExceptions) categoryCount++;

            var maximumWidthRequest = control.IsVisible
                ? (categoryCount * 10) + 10
                : 0;
            control.MaximumWidthRequest = maximumWidthRequest;

            control.InformationsImageButton.IsVisible = control.HasInformations;
            control.WarningsImageButton.IsVisible = control.HasInformations;
            control.ErrorsImageButton.IsVisible = control.HasInformations;
            control.UnhandledExceptionsImageButton.IsVisible = control.HasInformations;
        });
    public IList<IApiResultMessageModel> ApiResultMessages
    {
        get => (IList<IApiResultMessageModel>)GetValue(ApiResultMessagesProperty);
        set => SetValue(ApiResultMessagesProperty, value);
    }

    protected bool HasInformations => Informations.Any();

    protected bool HasWarnings => Warnings.Any();

    protected bool HasErrors => Errors.Any();

    protected bool HasUnhandledExceptions => UnhandledExceptions.Any();

    protected IEnumerable<IApiResultMessageModel> Informations =>
        ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Information));

    protected IEnumerable<IApiResultMessageModel> Warnings =>
        ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Warning));

    protected IEnumerable<IApiResultMessageModel> Errors =>
        ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Error));

    protected IEnumerable<IApiResultMessageModel> UnhandledExceptions =>
        ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.UnhandledException));

}