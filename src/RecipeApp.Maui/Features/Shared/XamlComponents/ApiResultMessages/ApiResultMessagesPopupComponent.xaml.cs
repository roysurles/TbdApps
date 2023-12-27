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
        });

    public IList<IApiResultMessageModel> ApiResultMessages { get; set; } = new List<IApiResultMessageModel>();

    public Border PopupBorder => ThisBorder;


    public static readonly BindableProperty InformationsProperty =
        BindableProperty.Create(nameof(Informations), typeof(IList<IApiResultMessageModel>), typeof(ApiResultMessagesPopupComponent));
    public IList<IApiResultMessageModel> Informations
    {
        get => (IList<IApiResultMessageModel>)GetValue(InformationsProperty);
        set => SetValue(InformationsProperty, value);
    }
    //protected IEnumerable<IApiResultMessageModel> Warnings =>
    //    ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Warning));

    //protected IEnumerable<IApiResultMessageModel> Errors =>
    //    ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.Error));

    //protected IEnumerable<IApiResultMessageModel> UnhandledExceptions =>
    //    ApiResultMessages.Where(x => x.MessageType.Equals(ApiResultMessageModelTypeEnumeration.UnhandledException));

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}