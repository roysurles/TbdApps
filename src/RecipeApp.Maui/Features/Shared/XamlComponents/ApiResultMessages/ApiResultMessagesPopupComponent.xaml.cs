namespace RecipeApp.Maui.Features.Shared.XamlComponents.ApiResultMessages;

public partial class ApiResultMessagesPopupComponent : Popup
{
    public ApiResultMessagesPopupComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ApiResultMessagesProperty =
        BindableProperty.Create(nameof(ApiResultMessages), typeof(IList<IApiResultMessageModel>), typeof(ApiResultMessagesPopupComponent), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (ApiResultMessagesPopupComponent)bindable;

            control.Informations = control.ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Information)).ToList();
            control.Warnings = control.ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Warning)).ToList();
            control.Errors = control.ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Error)).ToList();
            control.UnhandledExceptions = control.ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.UnhandledException)).ToList();
        });

    public IList<IApiResultMessageModel> ApiResultMessages
    {
        get => (IList<IApiResultMessageModel>)GetValue(ApiResultMessagesProperty);
        set => SetValue(ApiResultMessagesProperty, value);
    }

    public Border PopupBorder => ThisBorder;


    public static readonly BindableProperty InformationsProperty =
        BindableProperty.Create(nameof(Informations), typeof(IList<IApiResultMessageModel>), typeof(ApiResultMessagesPopupComponent));
    public IList<IApiResultMessageModel> Informations
    {
        get => (IList<IApiResultMessageModel>)GetValue(InformationsProperty);
        set => SetValue(InformationsProperty, value);
    }

    public static readonly BindableProperty WarningsProperty =
        BindableProperty.Create(nameof(Warnings), typeof(IList<IApiResultMessageModel>), typeof(ApiResultMessagesPopupComponent));
    public IList<IApiResultMessageModel> Warnings
    {
        get => (IList<IApiResultMessageModel>)GetValue(WarningsProperty);
        set => SetValue(WarningsProperty, value);
    }

    public static readonly BindableProperty ErrorsProperty =
        BindableProperty.Create(nameof(Errors), typeof(IList<IApiResultMessageModel>), typeof(ApiResultMessagesPopupComponent));
    public IList<IApiResultMessageModel> Errors
    {
        get => (IList<IApiResultMessageModel>)GetValue(ErrorsProperty);
        set => SetValue(ErrorsProperty, value);
    }

    public static readonly BindableProperty UnhandledExceptionsProperty =
        BindableProperty.Create(nameof(UnhandledExceptions), typeof(IList<IApiResultMessageModel>), typeof(ApiResultMessagesPopupComponent));
    public IList<IApiResultMessageModel> UnhandledExceptions
    {
        get => (IList<IApiResultMessageModel>)GetValue(UnhandledExceptionsProperty);
        set => SetValue(UnhandledExceptionsProperty, value);
    }

    private void Button_Clicked(object sender, EventArgs e) => Close();
}